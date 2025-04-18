﻿using FlightBooking.Entities.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightBooking.Infrastructure.Repositories.Interfaces
{
    public interface IAccountRepository
    {
        public Task<IdentityResult> SignUpAsync(ApplicationUser user);
        public Task<IdentityUser> SignInAsync(string email, string password);
        public Task<ApplicationUser> FindByEmailAsync(string email);
        public Task<ApplicationUser> FindByIdAsync(string id);
        public Task<IEnumerable<string>> GetRolesAsync(ApplicationUser user);
        public Task<IdentityUser> SignOutAsync();

    }
}
