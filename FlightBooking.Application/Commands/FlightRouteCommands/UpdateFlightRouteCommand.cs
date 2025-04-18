using FlightBooking.Application.DTOs;
using FlightBooking.Entities.Entities;
using FlightBooking.Infrastructure.Repositories.IRepositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightBooking.Application.Commands.FlightRouteCommands
{
    public class UpdateFlightRouteCommand : IRequest<bool>
    {
        public FlightRouteDTO FlightRouteDTO { get; set; }
        public class UpdateFlightRouteCommandHandler : IRequestHandler<UpdateFlightRouteCommand, bool>
        {
            private readonly IFlightRouteRepository _flightRoute;

            public UpdateFlightRouteCommandHandler(IFlightRouteRepository flightRoute)
            {
                _flightRoute = flightRoute;
            }
            public async Task<bool> Handle(UpdateFlightRouteCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    var fRoute = new TuyenBay
                    {
                        MaTuyenBay = (int)request.FlightRouteDTO.MaTuyenBay,
                        MaThanhPhoDi = request.FlightRouteDTO.MaThanhPhoDi,
                        MaThanhPhoDen = request.FlightRouteDTO.MaThanhPhoDen,
                        KhoangCach = request.FlightRouteDTO.KhoangCach,
                        MaSanBay = request.FlightRouteDTO.MaSanBay,
                    };
                    await _flightRoute.UpdateAsync(fRoute);
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }
    }
}
