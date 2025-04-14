using FlightBooking.Entities.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightBooking.Application.Services.IServices
{
    public interface IAccountService
    {
        public Task<IdentityResult> SignUpAsync(ApplicationUser user);
        public Task<IdentityUser> SignInAsync(string email, string password);
        public Task<IdentityUser> SignOutAsync();
    }
}
