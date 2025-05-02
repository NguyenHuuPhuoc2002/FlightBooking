using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightBooking.Entities.Entities
{
    public class ChiTietLienHe
    {
        public int MaChiTietLH { get; set; }
        public string HoTen { get; set; }
        public string Email { get; set; }
        public string SDT { get; set; }
        public string DanhXung { get; set; }
        public Guid? MaVe { get; set; }
        public Ve Ve { get; set; }
    }
}
