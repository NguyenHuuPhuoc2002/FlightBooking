using FlightBooking.Infrastructure.DbContext;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightBooking.Infrastructure.Seeding
{
    public class SeedData
    {
        public static async Task Seed(DataContext context, RoleManager<IdentityRole> roleManager)
        {
            if (!await roleManager.RoleExistsAsync("Admin"))
            {
                var adminRole = new IdentityRole("Admin");
                var result = await roleManager.CreateAsync(adminRole);
            }

            // Kiểm tra nếu vai trò "Customer" chưa tồn tại
            if (!await roleManager.RoleExistsAsync("Customer"))
            {
                var customerRole = new IdentityRole("Customer");
                var result = await roleManager.CreateAsync(customerRole);

            }
            await context.SaveChangesAsync();
        }
    }
}
