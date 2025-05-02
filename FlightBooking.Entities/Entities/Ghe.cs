using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightBooking.Entities.Entities
{
    public class Ghe
    {
        public string MaGhe {  get; set; }
        public string MaLoaiGhe{ set; get; }
        public LoaiGhe LoaiGhe{  get; set; }
        public ICollection<Ve> Ves { get; set; }
    }
}
