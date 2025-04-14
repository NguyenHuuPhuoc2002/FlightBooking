﻿using FlightBooking.Infrastructure.Repositories;
using FlightBooking.Infrastructure.Repositories.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightBooking.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationMediaR(this IServiceCollection services)
        {
            services.AddMediatR(cf => cf.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));

            services.AddTransient<IAccountRepository, AccountRepository>();

            return services;

        }
    }

}
