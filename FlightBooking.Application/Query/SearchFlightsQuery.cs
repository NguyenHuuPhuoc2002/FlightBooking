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
    public class SearchFlightsQuery: IRequest<IEnumerable<FlightDTO>>
    {
        public int? Page { get; set; }
        public int? PageSize { get; set; }
        public string? DiemDen { get; set; }
        public string? DiemDi { get; set; }
        public DateOnly? NgayKhoiHanh { get; set; }
        public class SearchFlightsQueryHandler : IRequestHandler<SearchFlightsQuery, IEnumerable<FlightDTO>>
        {
            private readonly IFlightRepository _flight;

            public SearchFlightsQueryHandler(IFlightRepository flight)
            {
                _flight = flight;
            }
            public async Task<IEnumerable<FlightDTO>> Handle(SearchFlightsQuery request, CancellationToken cancellationToken)
            {
                try
                {
                    int pageIndex = request.Page ?? 1;
                    int pSize = request.PageSize ?? 5;
                    var data = await _flight.SearchFlightsAsync(request.DiemDen, request.DiemDi, (DateOnly)request.NgayKhoiHanh, pageIndex, pSize);
                    var result = data.Select(e => new FlightDTO
                    {
                        MaChuyenBay = e.MaChuyenBay,
                        GioBay = e.GioBay,
                        GioDen = e.GioDen,
                        GiaVe = e.GiaVe,
                        NgayBay = e.NgayBay,
                        TenTrangThai = EnumHelper.GetDisplayName(e.TrangThai),
                        TenMayBay = e.MayBay.TenMayBay,
                        TuyenBay = e.TuyenBay.ThanhPhoDi.TenThanhPho + " -> " + e.TuyenBay.ThanhPhoDen,
                    });
                    return result;
                }
                catch (Exception ex)
                {
                    throw new AppException($"Lỗi server: {ex.Message}", 500);
                }
            }
        }
    }
}
