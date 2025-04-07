using System;
using System.Collections.Generic;
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
        public Decimal GiaVe {  get; set; }
        public TrangThaiChuyenBay TrangThai { get; set; }
        public int MaMayBay {  get; set; }
        public MayBay MayBay { get; set; }
        public int MaTuyenBay { get; set; }
        public TuyenBay TuyenBay{ get; set; }
        public ICollection<GheChuyenBay> GheChuyenBays { get; set; }
        public ICollection<Ve> ves { get; set; }

    }
    public enum TrangThaiChuyenBay
    {
        DangKhoiHanh,
        DaDen,
        HuyChuyen,
    }

}
