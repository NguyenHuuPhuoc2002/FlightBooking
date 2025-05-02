using FlightBooking.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightBooking.Application.DTOs
{
    public class FlightDTO
    {
        public int? MaChuyenBay { get; set; }
        public TimeOnly GioBay { get; set; }
        public TimeOnly GioDen { get; set; }
        public DateOnly NgayBay { get; set; }
        public float GiaVe{ get; set; }
        public int MaMayBay { get; set; }
        public int MaTuyenBay { get; set; }
        public TrangThaiChuyenBay? MaTrangThai{ get; set; }
        public string? TenTrangThai { get; set; }
        public string? TenMayBay { get; set; }
        public string? TuyenBay { get; set; }
    }
}
