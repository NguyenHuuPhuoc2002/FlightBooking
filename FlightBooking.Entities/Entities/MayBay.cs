using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightBooking.Entities.Entities
{
    public class MayBay
    {
        public int MaMayBay { get; set; }
        public string TenMayBay { get; set; }
        public string HangHangKhong {  get; set; }
        public int SoChoNgoi { get; set; }
        public int SoChoNgoiPhoThong { get; set; }
        public int SoChoNgoiThuongGia { get; set; }
        public ICollection<ChuyenBay> ChuyenBays { get; set; }
    }
}
