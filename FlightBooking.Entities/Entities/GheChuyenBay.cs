using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightBooking.Entities.Entities
{
    public class GheChuyenBay
    {
        public int MaGheChuyenBay { get; set; }
        public TrangThaiGheChuyenBay TrangThai { get; set; }
        public int MaChuyenBay { get; set; }
        public ChuyenBay ChuyenBay { get; set; }
        public int MaGhe { get; set; }
        public Ghe Ghe { get; set; }
    }
    public enum TrangThaiGheChuyenBay
    {
        Trong,
        DaDat
    }
}
