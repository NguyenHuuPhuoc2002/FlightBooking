using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightBooking.Entities.Entities
{
    public class Ghe
    {
        public int MaGhe {  get; set; }
        public string LoaiGhe {  set; get; }
        public float HeSoGia {  get; set; }
        public ICollection<GheChuyenBay> GheChuyenBays { get; set; }
    }
}
