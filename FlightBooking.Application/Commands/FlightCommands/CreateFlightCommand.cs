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
                    var Flight = new ChuyenBay
                    {
                       GioBay = request.FlightDTO.GioBay,
                       GioDen = request.FlightDTO.GioDen,
                       NgayBay = request.FlightDTO.NgayBay,
                       GiaVe = request.FlightDTO.GiaVe,
                       TrangThai = request.FlightDTO.MaTrangThai,
                       MaMayBay = request.FlightDTO.MaMayBay,
                       MaTuyenBay = request.FlightDTO.MaTuyenBay,
                    };
                    await _flight.CreateAsync(Flight);

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
