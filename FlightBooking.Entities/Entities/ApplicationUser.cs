using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightBooking.Entities.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public int? Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string CCCD{ get; set; }
        public int DiemTichLuy{ get; set; }
        public string? Image { get; set; }
    }
}
