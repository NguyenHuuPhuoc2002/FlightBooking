using FlightBooking.Entities.Entities;
using FlightBooking.Infrastructure.DbContext;
using FlightBooking.Infrastructure.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shared.Exceptions;
using Shared.PaginatedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Reflection.Metadata.BlobBuilder;

namespace FlightBooking.Infrastructure.Repositories
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
                _logger.LogInformation("Kiểm tra tuyến bay tồn tại hay chưa !");
                var isExists = await _context.TuyenBays.AnyAsync(tb =>
                                tb.MaThanhPhoDi == model.MaThanhPhoDi &&
                                tb.MaThanhPhoDen == model.MaThanhPhoDen &&
                                tb.MaSanBay == model.MaSanBay);
                if (isExists)
                {
                    return false;
                }
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
                if (findFRoute == null)
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
                findFRoute.MaThanhPhoDen = model.MaThanhPhoDen;
                findFRoute.MaThanhPhoDi = model.MaThanhPhoDi;
                findFRoute.MaSanBay = model.MaSanBay;
                findFRoute.KhoangCach = model.KhoangCach;
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

        public async Task<IEnumerable<TuyenBay>> GetAllAsync(int page, int pageSize)
        {
            try
            {
                _logger.LogInformation("Lấy tất cả tuyến bay !");
                var listFRoute = await _context.TuyenBays.Include(e => e.ThanhPhoDen)
                                                         .Include(e => e.ThanhPhoDi)
                                                         .Include(e => e.SanBay)
                                                         .ToListAsync();
                var data = PaginatedList<TuyenBay>.Create(listFRoute.AsQueryable(), page, pageSize);
                return data;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Xóa tuyến bay thất bại !: {ex.Message}");
                throw new AppException($"Lỗi server{ex.Message}", 500);
            }
        }
    }
}
