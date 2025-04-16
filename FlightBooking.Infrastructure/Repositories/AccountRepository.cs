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
                if (user == null)
                {
                    _logger.LogWarning("User không tồn tại!");
                    throw new KeyNotFoundException("User không tồn tại!");
                }
                var passwordValid = await _userManager.CheckPasswordAsync(user, password);
                if (!passwordValid)
                {
                    _logger.LogWarning("Sai mật khẩu!");
                    throw new UnauthorizedAccessException("Sai mật khẩu!");
                }
                var result = await _signInManager.PasswordSignInAsync(user, password, false, false);
                if (!result.Succeeded)
                {
                    _logger.LogWarning("Đăng nhập không thành công!");
                    throw new UnauthorizedAccessException("Đăng nhập không thành công!");
                }
                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Xảy ra lỗi khi kiểm tra đăng nhập!");
                throw; 
            }
        }

        public async Task<IdentityResult> SignUpAsync(ApplicationUser user)
        {
            try
            {
                _logger.LogInformation("Tạo mới user !");
                var result = await _userManager.CreateAsync(user, user.PasswordHash);

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
        public async Task<ApplicationUser> FindByEmailAsync(string email)
        {
            try
            {
                _logger.LogInformation($"Không tìm thấy user {email}");
                var user = await _userManager.FindByEmailAsync(email);
                if (user == null)
                {
                    _logger.LogWarning($"Không tìm thấy user {email}");
                    throw new KeyNotFoundException("Không tìm thấy user !");
                }
                return user;
            }
            catch (Exception)
            {
                _logger.LogError("Xảy ra lỗi khi tìm kiếm user");
                throw;
            }
        }

        public async Task<ApplicationUser> FindByIdAsync(string id)
        {
            try
            {
                _logger.LogInformation($"Không tìm thấy user {id}");
                var user = await _userManager.FindByIdAsync(id);
                if (user == null)
                {
                    _logger.LogWarning($"Không tìm thấy user {id}");
                    throw new KeyNotFoundException("Không tìm thấy user !");
                }
                return user;
            }
            catch (Exception)
            {
                _logger.LogError("Xảy ra lỗi khi tìm kiếm user");
                throw;
            }
        }
        public async Task<IEnumerable<string>> GetRolesAsync(ApplicationUser user)
        {
            try
            {
                _logger.LogInformation("Thực hiện truy vấn lấy các role của user {email}", user.Email);
                var useRole = await _userManager.GetRolesAsync(user);
                return useRole;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, "Xảy ra lỗi khi lấy role của user {email}", user.Email);
                throw;
            }
        }
        public Task<IdentityUser> SignOutAsync()
        {
            throw new NotImplementedException();
        }

    }
}
