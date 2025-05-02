using FlightBooking.Application.DTOs;
using FlightBooking.Entities.Entities;
using FlightBooking.Infrastructure.Repositories.IRepositories;
using MediatR;
using Shared.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightBooking.Application.Commands.BookingTicketCommands
{
    public class BookingCommand: IRequest<bool>
    {
        public required BookingTichketDTO BookingTichketDTO { get; set; }
        public class BookingCommandHandler : IRequestHandler<BookingCommand, bool>
        {
            private readonly IBookTicketRepository _bookTicket;
            private readonly IFlightRepository _flight;

            public BookingCommandHandler(IBookTicketRepository bookTicket, IFlightRepository flight)
            {
                _bookTicket = bookTicket;
                _flight = flight;
            }
            public async Task<bool> Handle(BookingCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    var listFight = await _bookTicket.Gets();
                    var check = listFight.Any(e =>
                            e.MaGhe == request.BookingTichketDTO.MaGhe &&
                            e.MaChuyenBay == request.BookingTichketDTO.MaChuyenBay);
                    if (check)
                    {
                        throw new AppException("Ghế đã được đặt !", 400);
                    }
                    Guid randomGuid = Guid.NewGuid();
                    var ve = new Ve
                    {
                        MaVe = randomGuid,
                        MaChuyenBay = request.BookingTichketDTO.MaChuyenBay,
                        NgayDatVe = DateTime.Now,
                        TrangThai = TrangThaiVe.DaDat,
                        MaThanhVien = (int)request.BookingTichketDTO.MaThanhVien,
                        MaGhe = request.BookingTichketDTO.MaGhe,
                        MaGiamGia = null,
                    };
                    var chiTietLH = new ChiTietLienHe
                    {
                        SDT = request.BookingTichketDTO.SDT,
                        Email = request.BookingTichketDTO.Email,
                        HoTen = request.BookingTichketDTO.HoTen,
                        DanhXung = request.BookingTichketDTO.DanhXung,
                        MaVe = ve.MaVe,
                    };
                    var chiTietDichVu = new ChiTietDichVu
                    {
                        MaVe = ve.MaVe,
                        MaDichVu = (int)request.BookingTichketDTO.MaDichVu,
                    };
                    var result1 = await _bookTicket.BooKing(ve);
                    if (result1)
                    {
                        var ctlh = await _bookTicket.AddChiTietLH(chiTietLH);
                        if (request.BookingTichketDTO.MaDichVu > 0)
                        {
                            var dv = await _bookTicket.AddDichVu(chiTietDichVu);
                        }
                        return true;
                    }
                    
                    return false;
                }
                catch (Exception ex)
                {
                    throw new AppException($"Lỗi: {ex.Message}", 500);
                }
            }
        }
    }
}
