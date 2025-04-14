using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightBooking.Application.DTOs
{
    public class SignUpDTO
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required, EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string ConfirmPassword { get; set; }
        public DateTime? DayOfBirth { get; set; }
        public int? Gender { get; set; }
        [Required]
        public string CCCD { get; set; }
        public IFormFile? Image { get; set; }
        public string? Hinh { get; set; }
    }
}
