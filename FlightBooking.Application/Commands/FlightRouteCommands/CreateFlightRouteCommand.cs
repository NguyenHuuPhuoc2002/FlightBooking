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

namespace FlightBooking.Application.Commands.FlightRouteCommands
{
    public class CreateFlightRouteCommand: IRequest<bool>
    {
        public FlightRouteDTO? FlightRouteDTO { get; set; }
        public class CreateFlightRouteCommandHandler : IRequestHandler<CreateFlightRouteCommand, bool>
        {
            private readonly IFlightRouteRepository _flightRoute;
            public CreateFlightRouteCommandHandler(IFlightRouteRepository flightRoute)
            {
                _flightRoute = flightRoute;
            }
            public async Task<bool> Handle(CreateFlightRouteCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    var fRoute = new TuyenBay
                    {
                        MaThanhPhoDi = request.FlightRouteDTO.MaThanhPhoDi,
                        MaThanhPhoDen = request.FlightRouteDTO.MaThanhPhoDen,
                        KhoangCach = request.FlightRouteDTO.KhoangCach,
                        MaSanBay = request.FlightRouteDTO.MaSanBay,
                    };
                    await _flightRoute.AddAsync(fRoute);

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
