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
    public class DeleteFlightCommand: IRequest<bool>
    {
        public int MaChuyenBay { get; set; }
        public class DeleteFlightCommandHandler : IRequestHandler<DeleteFlightCommand, bool>
        {
            private readonly IFlightRepository _flight;

            public DeleteFlightCommandHandler(IFlightRepository flight)
            {
                _flight = flight;
            }
            public async Task<bool> Handle(DeleteFlightCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    var result = await _flight.DeleteAsync(request.MaChuyenBay);
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
