using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightBooking.Entities.Entities
{
    public class ChuyenBay
    {
        public int MaChuyenBay { get; set; }
        public TimeOnly GioBay { get; set; }
        public TimeOnly GioDen {  get; set; }
        public DateOnly NgayBay { get; set; }
        public float GiaVe {  get; set; }
        public TrangThaiChuyenBay TrangThai { get; set; }
        public int MaMayBay {  get; set; }
        public MayBay MayBay { get; set; }
        public int MaTuyenBay { get; set; }
        public TuyenBay TuyenBay{ get; set; }
        public ICollection<Ve> ves { get; set; }

    }
    public enum TrangThaiChuyenBay
    {
        [Display(Name = "Đang khởi hành")]
        DangKhoiHanh,
        [Display(Name = "Đã đến")]
        DaDen,
        [Display(Name = "Hủy chuyến")]
        HuyChuyen
    }

}
