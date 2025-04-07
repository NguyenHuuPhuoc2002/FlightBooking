using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightBooking.Entities.Entities
{
    public class SanBay
    {
        public int MaSanBay { get; set; }
        public string TenSanBay { get; set; }
        public string DiaChi { get; set; }
        public int MaThanhPho { get; set; }
        public ThanhPho ThanhPho { get; set; }
        public ICollection<TuyenBay> TuyenBays { get; set; }

    }
}
