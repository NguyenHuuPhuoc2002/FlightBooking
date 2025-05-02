using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightBooking.Application.DTOs
{
    public class BookingTichketDTO
    {
        public int MaChuyenBay { get; set; }
        public int? MaThanhVien { get; set; }
        public string HoTen { get; set; }
        public string Email { get; set; }
        public string SDT { get; set; }
        public string DanhXung { get; set; }
        public string MaGhe { get; set; }
        public int? MaGiamGia { get; set; }
        public int? MaDichVu { get; set; }
    }
}
