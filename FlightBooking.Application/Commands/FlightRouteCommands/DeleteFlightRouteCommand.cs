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
    public class DeleteFlightRouteCommand: IRequest<bool>
    {
        public int MaTuyenBay {  get; set; }
        public class DeleteFlightRouteCommandHandler : IRequestHandler<DeleteFlightRouteCommand, bool>
        {
            private readonly IFlightRouteRepository _flightRoute;

            public DeleteFlightRouteCommandHandler(IFlightRouteRepository flightRoute)
            {
                _flightRoute = flightRoute;
            }

            public async Task<bool> Handle(DeleteFlightRouteCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    var deleteFRoute = await _flightRoute.DeleteAsync(request.MaTuyenBay);
                    if (!deleteFRoute)
                    {
                        return false;
                    }
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }
    }
}
