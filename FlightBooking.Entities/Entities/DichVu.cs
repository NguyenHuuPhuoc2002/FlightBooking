using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightBooking.Entities.Entities
{
    public class DichVu
    {
        public int MaDichVu {  get; set; }
        public string TenDichVu {  set; get; }
        public decimal GiaDichVu { set; get; }
        public ICollection<ChiTietDichVu> ChiTietDichVus { get; set; }
    }
}
