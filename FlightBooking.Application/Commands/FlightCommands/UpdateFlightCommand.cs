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
    public class UpdateFlightCommand: IRequest<bool>
    {
        public FlightDTO FlightDTO { get; set; }
        public UpdateFlightCommand()
        {

        }
        public class UpdateFlightCommandHandler : IRequestHandler<UpdateFlightCommand, bool>
        {
            private readonly IFlightRepository _flight;

            public UpdateFlightCommandHandler(IFlightRepository flight)
            {
                _flight = flight;
            }
            public async Task<bool> Handle(UpdateFlightCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    var currentDate = DateOnly.FromDateTime(DateTime.Now);
                    var currentTime = TimeOnly.FromDateTime(DateTime.Now);
                    if (request.FlightDTO.NgayBay < currentDate)
                        throw new AppException("Ngày bay phải lớn hơn hoặc bằng ngày hiện tại !", 400);
                    if (request.FlightDTO.GioBay <= currentTime && request.FlightDTO.NgayBay < currentDate)
                        throw new AppException("Giờ bay phải lớn hơn giờ hiện tại !", 400);
                    var data = new ChuyenBay
                    {
                        MaChuyenBay = (int)request.FlightDTO.MaChuyenBay,
                        GioBay = request.FlightDTO.GioBay,
                        GioDen = request.FlightDTO.GioDen,
                        NgayBay = request.FlightDTO.NgayBay,
                        GiaVe = request.FlightDTO.GiaVe,
                        TrangThai = (TrangThaiChuyenBay)request.FlightDTO.MaTrangThai,
                        MaMayBay = request.FlightDTO.MaMayBay,
                        MaTuyenBay = request.FlightDTO.MaTuyenBay,
                    };
                    var result = await _flight.UpdateAsync(data);
                    if (!result)
                        throw new AppException("Chuyến bay đã tồn tại !", 409);
                    return true;
                }
                catch (Exception ex)
                {
                    throw new AppException($"Lỗi server: {ex.Message}", 500);
                }
            }
        }
    }
}
