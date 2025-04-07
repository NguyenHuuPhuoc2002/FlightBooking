using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightBooking.Entities.Entities
{
    public class TuyenBay
    {
        public int MaTuyenBay { get; set; }
        public int MaThanhPhoDen { get; set; }
        public int MaThanhPhoDi { get; set; }
        public float KhoangCach { get; set; }
        public int MaSanBay { get; set; }
        public SanBay SanBay { get; set; }
        public ICollection<ChuyenBay> ChuyenBays { get; set; }
    }
}
