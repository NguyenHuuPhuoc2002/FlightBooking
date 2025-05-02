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

namespace FlightBooking.Application.Commands.FlightCommands
{
    public class CreateFlightCommand : IRequest<bool>
    {
        public FlightDTO FlightDTO { get; set; }

        public class CreateFlightCommandHandler : IRequestHandler<CreateFlightCommand, bool>
        {
            private readonly IFlightRepository _flight;

            public CreateFlightCommandHandler(IFlightRepository flight) 
            {
                _flight = flight;
            }
            public async Task<bool> Handle(CreateFlightCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    var currentDate = DateOnly.FromDateTime(DateTime.Now);
                    var currentTime = TimeOnly.FromDateTime(DateTime.Now);
                    if (request.FlightDTO.NgayBay < currentDate)
                        throw new AppException("Ngày bay phải lớn hơn hoặc bằng ngày hiện tại !", 400);
                    if (request.FlightDTO.GioBay <= currentTime && request.FlightDTO.NgayBay < currentDate)
                        throw new AppException("Giờ bay phải lớn hơn giờ hiện tại !", 400);
                    var Flight = new ChuyenBay
                    {
                       GioBay = request.FlightDTO.GioBay,
                       GioDen = request.FlightDTO.GioDen,
                       NgayBay = request.FlightDTO.NgayBay,
                       GiaVe = request.FlightDTO.GiaVe,
                       TrangThai = TrangThaiChuyenBay.DangKhoiHanh,
                       MaMayBay = request.FlightDTO.MaMayBay,
                       MaTuyenBay = request.FlightDTO.MaTuyenBay,
                    };
                    var check = await _flight.CreateAsync(Flight);
                    if (!check)
                        throw new AppException("Chuyến bay đã tồn tại !", 409);
                    return true;
                }
                catch (Exception ex)
                {
                    throw new AppException($"Lỗi: {ex.Message}", 500);
                }
            }
        }
    }
}
