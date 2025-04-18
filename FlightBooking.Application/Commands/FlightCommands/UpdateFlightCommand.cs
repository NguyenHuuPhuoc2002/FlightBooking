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
                    var data = new ChuyenBay
                    {
                        MaChuyenBay = (int)request.FlightDTO.MaChuyenBay,
                        GioBay = request.FlightDTO.GioBay,
                        GioDen = request.FlightDTO.GioDen,
                        NgayBay = request.FlightDTO.NgayBay,
                        GiaVe = request.FlightDTO.GiaVe,
                        TrangThai = request.FlightDTO.MaTrangThai,
                        MaMayBay = request.FlightDTO.MaMayBay,
                        MaTuyenBay = request.FlightDTO.MaTuyenBay,
                    };
                    var result = await _flight.UpdateAsync(data);
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
