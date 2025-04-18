using FlightBooking.Application.DTOs;
using FlightBooking.Infrastructure.Repositories.IRepositories;
using MediatR;
using Shared.Exceptions;
using Shared.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightBooking.Application.Query
{
    public class GetAllFlightQuery: IRequest<IEnumerable<FlightDTO>>
    {
        public int? Page { get; set; }
        public int? PageSize { get; set; }
        public GetAllFlightQuery()
        {
        }
        public class GetAllFlightQueryHandler : IRequestHandler<GetAllFlightQuery, IEnumerable<FlightDTO>>
        {
            private readonly IFlightRepository _flight;

            public GetAllFlightQueryHandler(IFlightRepository flight)
            {
                _flight = flight;
            }
            public async Task<IEnumerable<FlightDTO>> Handle(GetAllFlightQuery request, CancellationToken cancellationToken)
            {
                try
                {
                    int page = request.Page ?? 1;
                    int pSize = request.PageSize ?? 3;
                    var flights = await _flight.GetAllAsync(page, pSize);
                    var data = flights.Select(e => new FlightDTO
                    {
                        MaChuyenBay = e.MaChuyenBay,
                        GioBay = e.GioBay,
                        GioDen = e.GioDen,
                        NgayBay = e.NgayBay,
                        GiaVe = e.GiaVe,
                        TenTrangThai = EnumHelper.GetDisplayName(e.TrangThai),
                        TenMayBay = e.MayBay.TenMayBay,
                        TuyenBay = e.TuyenBay.ThanhPhoDi.TenThanhPho.ToString() + " -> " + e.TuyenBay.ThanhPhoDen.TenThanhPho.ToString(),
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
