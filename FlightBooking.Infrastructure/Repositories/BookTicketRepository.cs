using FlightBooking.Entities.Entities;
using FlightBooking.Infrastructure.DbContext;
using FlightBooking.Infrastructure.Repositories.IRepositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shared.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightBooking.Infrastructure.Repositories
{
    public class BookTicketRepository : IBookTicketRepository
    {
        private readonly DataContext _context;
        private readonly ILogger<BookTicketRepository> _logger;

        public BookTicketRepository(DataContext context, ILogger<BookTicketRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<bool> AddChiTietLH(ChiTietLienHe model)
        {
            try
            {
                await _context.ChiTietLienHes.AddAsync(model);
                var result = await _context.SaveChangesAsync();
                if(result > 0)
                {
                    return true;
                }
                return false;

            }
            catch (Exception ex)
            {
                _logger.LogError($"Lỗi thêm chi tiết liên hệ !: {ex.Message}");
                throw new AppException($"Lỗi server{ex.Message}", 500);
            }
        }

        public async Task<bool> AddDichVu(ChiTietDichVu model)
        {
            try
            {
                await _context.ChiTietDichVus.AddAsync(model);
                var result = await _context.SaveChangesAsync();
                if (result > 0)
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Lỗi thêm dịch vụ !: {ex.Message}");
                throw new AppException($"Lỗi server{ex.Message}", 500);
            }
        }

        public async Task<bool> BooKing(Ve model)
        {
            try
            {
                await _context.Ves.AddAsync(model);
                var result = await _context.SaveChangesAsync();
                if(result > 0)
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Lỗi đặt vé !: {ex.Message}");
                throw new AppException($"Lỗi server{ex.Message}", 500);
            }
        }

        public async Task<bool> Cancel(Guid id)
        {
            try
            {
                var tichket = await _context.Ves.FirstOrDefaultAsync(e => e.MaVe ==  id);
                tichket.TrangThai = TrangThaiVe.DaHuy;
                _context.Ves.Update(tichket);
                var result = await _context.SaveChangesAsync();
                if (result > 0)
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Lỗi hủy vé !: {ex.Message}");
                throw new AppException($"Lỗi server{ex.Message}", 500);
            }
        }

        public async Task<IEnumerable<Ve>> Gets()
        {
            try
            {
                _logger.LogError("Lấy danh sách vé !");
                var listTickket = await _context.Ves.ToListAsync();
                return listTickket;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Lỗi Lấy danh sách vé !: {ex.Message}");
                throw new AppException($"Lỗi server{ex.Message}", 500);
            }
        }
    }
}
