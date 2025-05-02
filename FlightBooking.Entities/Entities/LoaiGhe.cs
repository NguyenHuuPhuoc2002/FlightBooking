using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightBooking.Entities.Entities
{
    public class LoaiGhe
    {
        public string MaLoaiGhe { get; set; }
        public string TenLoaiGhe { get; set; }
        public float HeSoGia { get; set; }
        public ICollection<Ghe> Ghes { get; set; }
    }
}
