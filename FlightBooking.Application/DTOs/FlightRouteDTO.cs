using FlightBooking.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightBooking.Application.DTOs
{
    public class FlightRouteDTO
    {
        public int? MaTuyenBay {  get; set; }
        public string? ThanhPhoDen{ get; set; }
        public string? ThanhPhoDi { get; set; }
        public string? SanBay { get; set; }
        public float KhoangCach { get; set; }
        public int MaThanhPhoDen { get; set; }
        public int MaThanhPhoDi { get; set; }
        public int MaSanBay { get; set; }
    }
}
