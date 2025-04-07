using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightBooking.Entities.Entities
{
    public class ChiTietDichVu
    {
        public int MaChiTietDV {  get; set; }
        public int MaDichVu {  get; set; }
        public DichVu DichVu{ get; set; }
        public int MaVe {  get; set; }
        public Ve Ve { get; set; }
    }
}
