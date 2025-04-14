using FlightBooking.Entities.Entities;
using FlightBooking.Infrastructure.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Shared.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightBooking.Infrastructure.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<AccountRepository> _logger;

        public AccountRepository(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
                                RoleManager<IdentityRole> roleManager, ILogger<AccountRepository> logger) 
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _logger = logger;
        }
        public async Task<IdentityUser> SignInAsync(string email, string password)
        {
            try
            {
                _logger.LogInformation("Thực hiện kiểm tra đăng nhập !");
                var user = await _userManager.FindByNameAsync(email);
                var passWordValid = await _userManager.CheckPasswordAsync(user, password);
                if(user == null || !passWordValid)
                {
                    _logger.LogInformation("User không tồn tại");
                    throw new KeyNotFoundException("User không tồn tại !");
                }

                var result = await _signInManager.PasswordSignInAsync(email, password, false, false);
                if (!result.Succeeded)
                {
                    _logger.LogInformation("Thông tin đăng nhập không chính xác");
                    throw new KeyNotFoundException("Thông tin đăng nhập không chính xác !");
                }

                return user;
            }
            catch (Exception)
            {
                _logger.LogError("Xảy ra lỗi kiểm tra đăng nhập !");
                throw;
            }
        }
        public async Task<IdentityResult> SignUpAsync(ApplicationUser user)
        {
            try
            {
                _logger.LogInformation("Tạo mới user !");
                var result = await _userManager.CreateAsync(user);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, AppRole.CUSTOMER);
                }
                return result;
            }
            catch (Exception)
            {
                _logger.LogError("Xảy ra lỗi khi tạo mới user !");
                throw;
            }
        }

        public Task<IdentityUser> SignOutAsync()
        {
            throw new NotImplementedException();
        }

    }
}
