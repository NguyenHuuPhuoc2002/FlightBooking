using FlightBooking.Entities.Entities;
using FlightBooking.Infrastructure.DbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shared.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightBooking.Infrastructure.Repositories.IRepositories
{
    public class FlightRouteRepository : IFlightRouteRepository
    {
        private readonly ILogger<FlightRouteRepository> _logger;
        private readonly DataContext _context;

        public FlightRouteRepository(DataContext context, ILogger<FlightRouteRepository> logger)
        {
            _logger = logger;
            _context = context;
        }
        public async Task<bool> AddAsync(TuyenBay model)
        {
            try
            {
                _logger.LogInformation("Thêm tuyến bay vào db !");
                var addFRoute = await _context.TuyenBays.AddAsync(model);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Thêm tuyến bay vào db thất bại !: {ex.Message}");
                throw new AppException($"Lỗi server{ex.Message}", 500);
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var findFRoute = await _context.TuyenBays.FirstOrDefaultAsync(e => e.MaTuyenBay == id);
                if(findFRoute == null)
                {
                    throw new AppException("Không tìm thấy tuyến bay !", 404);
                }
                var deleteFRoute = _context.TuyenBays.Remove(findFRoute);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Xóa tuyến bay thành công !");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Xóa tuyến bay thất bại !: {ex.Message}");
                throw new AppException($"Lỗi server{ex.Message}", 500);
            }
        }

        public async Task<bool> UpdateAsync(TuyenBay model)
        {
            try
            {
                var findFRoute = await _context.TuyenBays.FirstOrDefaultAsync(e => e.MaTuyenBay == model.MaTuyenBay);
                if (findFRoute == null)
                {
                    throw new AppException("Không tìm thấy tuyến bay !", 404);
                }
                var updateFRoute = _context.TuyenBays.Update(findFRoute);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Cập nhật tuyến bay thành công !");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Xóa tuyến bay thất bại !: {ex.Message}");
                throw new AppException($"Lỗi server{ex.Message}", 500);
            }
        }

        public async Task<IEnumerable<TuyenBay>> GetAllAsync()
        {
            try
            {
                _logger.LogInformation("Lấy tất cả tuyến bay !");
                var listFRoute = await _context.TuyenBays.ToListAsync();
                return listFRoute;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Xóa tuyến bay thất bại !: {ex.Message}");
                throw new AppException($"Lỗi server{ex.Message}", 500);
            }
        }
    }
}
