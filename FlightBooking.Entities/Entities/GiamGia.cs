using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightBooking.Entities.Entities
{
    public class GiamGia
    {
        public int MaGiamGia {  get; set; }
        public float PhanTram {  get; set; }
        public ICollection<Ve> Ves { get; set; }
    }
}
