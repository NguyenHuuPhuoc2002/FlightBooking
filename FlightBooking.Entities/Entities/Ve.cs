using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightBooking.Entities.Entities
{
    public class Ve
    {
        public Guid MaVe {  get; set; }
        public DateTime NgayDatVe { get; set; }
        public TrangThaiVe TrangThai { get; set; }
        public string MaThanhVien {  get; set; }
        public int MaChuyenBay {  get; set; }
        public string MaGhe { get; set; }
        public Ghe Ghe { get; set; }
        public ChuyenBay ChuyenBay { get; set; }
        public int? MaGiamGia { get; set; }
        public GiamGia GiamGia { get; set; }
        public ChiTietLienHe ChiTietLienHe { get; set; } 
        public ICollection<ChiTietDichVu> ChiTietDichVus { get; set; }
    }

    public enum TrangThaiVe
    {
        DaDat, DaHuy
    }
}
