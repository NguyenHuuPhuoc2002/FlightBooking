using FlightBooking.Application.DTOs;
using FlightBooking.Infrastructure.Repositories.IRepositories;
using MediatR;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shared.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightBooking.Application.Query
{
    public class GetAllFlightRouteQuery: IRequest<IEnumerable<FlightRouteDTO>>
    {
        public int? Page {  get; set; }
        public int? PageSize { get; set; }
        public GetAllFlightRouteQuery(int? page, int? pageSize)
        {
            Page = page;
            PageSize = pageSize;
        }
        public class GetAllFlightsRouteQueryHandler : IRequestHandler<GetAllFlightRouteQuery, IEnumerable<FlightRouteDTO>>
        {
            private readonly IFlightRouteRepository _flightRoute;

            public GetAllFlightsRouteQueryHandler(IFlightRouteRepository flightRoute)
            {
                _flightRoute = flightRoute;
            }
            public async Task<IEnumerable<FlightRouteDTO>> Handle(GetAllFlightRouteQuery request, CancellationToken cancellationToken)
            {
                try
                {
                    int page = request.Page ?? 1;
                    int pSize = request.PageSize ?? 3;
                    var listFRoute = await _flightRoute.GetAllAsync(page, pSize);
                    var data = listFRoute.Select(e => new FlightRouteDTO
                    {
                        MaTuyenBay = e.MaTuyenBay,
                        ThanhPhoDi = e.ThanhPhoDi.TenThanhPho,
                        ThanhPhoDen = e.ThanhPhoDen.TenThanhPho,
                        SanBay = e.SanBay.TenSanBay,
                        KhoangCach = e.KhoangCach,
                    });
                    return data;
                }
                catch (Exception ex)
                {
                    throw new AppException($"Lỗi server: {ex.Message}", 500);
                }
            }
        }
    }
}
