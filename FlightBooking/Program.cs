using FlightBooking.Application;
using FlightBooking.Application.DTOs;
using FlightBooking.Application.Services.IServices;
using FlightBooking.Application.Services;
using FlightBooking.Entities.Entities;
using FlightBooking.Infrastructure.DbContext;
using FlightBooking.Infrastructure.Seeding;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System;
using System.Text;
using Shared.Helpers;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

#region CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder => builder.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader());
});
#endregion

#region Logging
Console.OutputEncoding = System.Text.Encoding.UTF8;

//Cấu hình logging
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File(
        path: Path.Combine("..", "FlightBooking.Infrastructure", "Logs", "log.txt"),
        rollingInterval: RollingInterval.Day,
        retainedFileCountLimit: 7,
        encoding: System.Text.Encoding.UTF8)
    .CreateLogger();

#endregion

#region Connect MySQL
var conn = builder.Configuration.GetConnectionString("Database");
builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseMySql(conn, ServerVersion.AutoDetect(conn), b => b.MigrationsAssembly("FlightBooking.API"));
});
#endregion
#region setup cache
var cacheDurationInHours = builder.Configuration.GetValue<int>("CacheSettings:CacheDurationInHours");
var cacheSlidingExpirationInMinutes = builder.Configuration.GetValue<int>("CacheSettings:SlidingExpirationInMinutes");
var cacheDuration = TimeSpan.FromHours(cacheDurationInHours);
var cacheSlidingExpiration = TimeSpan.FromMinutes(cacheSlidingExpirationInMinutes);
builder.Services.AddSingleton(new CacheSetting
{
    Duration = cacheDuration,
    SlidingExpiration = cacheSlidingExpiration
});
builder.Services.AddMemoryCache(options =>
{
    options.SizeLimit = 10240; // Giới hạn tổng kích thước bộ nhớ cache (10 MB)
});
#endregion

#region redis custom
var redisConfiguration = new RedisConfiguration();
builder.Configuration.GetSection("RedisConfiguration").Bind(redisConfiguration);
if (string.IsNullOrEmpty(redisConfiguration.ConnectionString) || !redisConfiguration.Enabled)
{
    return;
}
builder.Services.AddSingleton(redisConfiguration);
builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
{
    return ConnectionMultiplexer.Connect(redisConfiguration.ConnectionString);
});
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = redisConfiguration.ConnectionString;
});
builder.Services.AddSingleton<IResponseCacheService, ResponseCacheService>();
builder.Services.Configure<RedisConfiguration>(builder.Configuration.GetSection("RedisConfiguration"));

#endregion
#region Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<DataContext>()
    .AddDefaultTokenProviders();
//authentication - Token 
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;


}).AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidAudience = builder.Configuration["JWT:ValidAudience"],
        ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"])),
        ClockSkew = TimeSpan.Zero
    };
});
#endregion

//dependency
builder.Services.AddSingleton<IVnPayService, VnPayService>();


builder.Services.Configure<AppSetting>(builder.Configuration.GetSection("JWT"));
builder.Host.UseSerilog();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    // Cấu hình để sử dụng Bearer Token trong Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });

});
builder.Services.AddApplicationMediaR();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAllOrigins");
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

var scope = app.Services.CreateScope();
var context = scope.ServiceProvider.GetRequiredService<DataContext>();
var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

try
{
    await context.Database.MigrateAsync();
    await SeedData.Seed(context, roleManager);
}
catch (Exception ex)
{
    logger.LogError(ex, "A problem occurred during migration");
}

app.Run();
