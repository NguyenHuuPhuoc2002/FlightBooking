using FlightBooking.Application.Services.IServices;
using FlightBooking.Entities.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightBooking.Application.Services
{
    public class AccountService : IAccountService
    {
        private readonly ILogger<IAccountService> _logger;

        public AccountService(ILogger<IAccountService> logger) 
        {
            _logger = logger;
        }
        public Task<IdentityUser> SignInAsync(string email, string password)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityUser> SignOutAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IdentityResult> SignUpAsync(ApplicationUser user)
        {
            throw new NotImplementedException();
        }
    }
}
