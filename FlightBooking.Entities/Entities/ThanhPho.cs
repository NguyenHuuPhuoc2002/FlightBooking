using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightBooking.Entities.Entities
{
    public class ThanhPho
    {
        public int MaThanhPho { get; set; }
        public string TenThanhPho { get; set; }
        public ICollection<SanBay> SanBays { get; set; }
    }
}
