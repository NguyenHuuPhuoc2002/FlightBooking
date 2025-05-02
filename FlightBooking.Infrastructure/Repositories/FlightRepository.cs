using FlightBooking.Entities.Entities;
using FlightBooking.Infrastructure.DbContext;
using FlightBooking.Infrastructure.Repositories.IRepositories;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shared.Exceptions;
using Shared.PaginatedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightBooking.Infrastructure.Repositories
{
    public class FlightRepository : IFlightRepository
    {
        private readonly ILogger<FlightRepository> _logger;
        private readonly DataContext _context;

        public FlightRepository(DataContext context, ILogger<FlightRepository> logger)
        {
            _logger = logger;
            _context = context;
        }
        public async Task<bool> CreateAsync(ChuyenBay model)
        {
            try
            {
                _logger.LogInformation("Thực hiện tạo chuyến bay !");
                var isDuplicate = await _context.ChuyenBays.AnyAsync(cb => (cb.MaTuyenBay == model.MaTuyenBay &&
                                                                           cb.NgayBay == model.NgayBay &&
                                                                           cb.GioBay == model.GioBay &&
                                                                           cb.MaMayBay == model.MaMayBay &&
                                                                           cb.TrangThai == TrangThaiChuyenBay.DangKhoiHanh));
                if (isDuplicate)
                {
                    return false;
                }
                var createFlight = await _context.ChuyenBays.AddAsync(model);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Tạo chuyến bay thất bại !: {ex.Message}");
                throw new AppException($"Lỗi server{ex.Message}", 500);
            }
        }
        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                _logger.LogInformation("Thực hiện xóa chuyến bay !");
                var findFlight = await _context.ChuyenBays.FirstOrDefaultAsync(e => e.MaChuyenBay == id);
                var deleteFlight = _context.ChuyenBays.Remove(findFlight);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Xóa chuyến bay thất bại !: {ex.Message}");
                throw new AppException($"Lỗi server{ex.Message}", 500);
            }
        }
        public async Task<bool> UpdateAsync(ChuyenBay model)
        {
            try
            {
                _logger.LogInformation("Thực hiện cập nhật chuyến bay !");
                var findFlight = await _context.ChuyenBays.FirstOrDefaultAsync(e => e.MaChuyenBay == model.MaChuyenBay);
                if (findFlight == null)
                {
                    throw new AppException("Không tìm thấy chuyến bay !", 404);
                }

                findFlight.GioBay = model.GioBay;
                findFlight.GioDen = model.GioDen;
                findFlight.NgayBay = model.NgayBay;
                findFlight.GiaVe = model.GiaVe;
                findFlight.TrangThai = model.TrangThai;
                findFlight.MaMayBay = model.MaMayBay;
                findFlight.MaTuyenBay = model.MaTuyenBay;

                var updateFlight = _context.ChuyenBays.Update(findFlight);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Cập nhật chuyến bay thất bại !: {ex.Message}");
                throw new AppException($"Lỗi server{ex.Message}", 500);
            }
        }
        public async Task<IEnumerable<ChuyenBay>> GetAllAsync(int page, int pageSize)
        {
            try
            {
                _logger.LogInformation("Lấy tất cả chuyến bay !");
                var listFRoute = await _context.ChuyenBays.Include(e => e.MayBay)
                                                         .Include(e => e.TuyenBay)
                                                         .ThenInclude(e => e.ThanhPhoDi)
                                                         .Include(e => e.TuyenBay)
                                                         .ThenInclude(e => e.ThanhPhoDen)
                                                         .ToListAsync();
                var data = PaginatedList<ChuyenBay>.Create(listFRoute.AsQueryable(), page, pageSize);
                return data;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Lấy tất cả chuyến bay thất bại !: {ex.Message}");
                throw new AppException($"Lỗi server{ex.Message}", 500);
            }
        }
        public async Task<IEnumerable<ChuyenBay>> SearchFlightsAsync(string dDen, string dDi, DateOnly? timeKhoiHanh, int page, int pageSize)
        {
            try
            {
                _logger.LogInformation("Tìm kiếm chuyến bay !");
                var query = _context.ChuyenBays.Include(e => e.TuyenBay)
                                               .ThenInclude(e => e.ThanhPhoDi)
                                               .Include(e => e.TuyenBay)
                                               .ThenInclude(e => e.ThanhPhoDen)
                                               .Include(e => e.MayBay)
                                               .AsQueryable();

                if (!string.IsNullOrEmpty(dDen))
                {
                    query = query.Where(e => e.TuyenBay.ThanhPhoDen.TenThanhPho.Contains(dDen));
                }

                if (!string.IsNullOrEmpty(dDi))
                {
                    query = query.Where(e => e.TuyenBay.ThanhPhoDi.TenThanhPho.Contains(dDi));
                }

                if (timeKhoiHanh != null)
                {
                    query = query.Where(e => e.NgayBay == timeKhoiHanh);
                }

                var findFlights = await query.ToListAsync();

                return findFlights;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Tìm kiếm chuyến bay thất bại !: {ex.Message}");
                throw new AppException($"Lỗi server{ex.Message}", 500);
            }
        }
    }
}
