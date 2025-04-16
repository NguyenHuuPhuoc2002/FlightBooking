using FlightBooking.Entities.Entities;
using FlightBooking.Infrastructure.DbContext;
using FlightBooking.Infrastructure.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightBooking.Infrastructure.Repositories
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly ILogger<RefreshTokenRepository> _logger;
        private readonly DataContext _context;

        public RefreshTokenRepository(ILogger<RefreshTokenRepository> logger, DataContext context)
        {
            _logger = logger;
            _context = context;
        }
        public async Task<bool> AddAsync(RefreshToken token)
        {
            try
            {
                _logger.LogInformation("Thực hiện thêm refreshToken vào csdl");
                var addTokem = await _context.AddAsync(token);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Thực hiện thêm refreshToken vào csdl thành công");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, "Xảy ra lỗi khi thêm refreshToken vào csdl");
                throw;
            }
        }

        public async Task<RefreshToken> GetTokenAsync(string refreshToken)
        {
            try
            {
                _logger.LogInformation("Truy vấn lấy refreshToken");
                var storedToken = await _context.RefreshTokens.FirstOrDefaultAsync(x => x.Token == refreshToken);
                if (storedToken == null)
                {
                    _logger.LogWarning("Không tìm thấy refreshToken");
                    throw new KeyNotFoundException("Không tìm thấy refreshToken");
                }
                else
                {
                    _logger.LogInformation("Lấy refreshToken thành công");
                    return storedToken;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Xảy ra lỗi khi lấy token csdl");
                throw;
            }
        }

        public async Task<bool> UpdateAsync(RefreshToken token, string refreshToken)
        {
            try
            {
                _logger.LogInformation("Truy vấn lấy refreshToken");
                var storedToken = await _context.RefreshTokens.FirstOrDefaultAsync(x => x.Token == refreshToken);
                _logger.LogInformation("Thực hiện cập nhật token");
                _context.Update(token);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Thực hiện cập nhật token thành công");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, "Xảy ra lỗi khi cập nhật token ");
                throw;
            }
        }
    }
}
