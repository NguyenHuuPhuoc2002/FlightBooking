using FlightBooking.Application.DTOs;
using FlightBooking.Entities.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightBooking.Application.Services.IServices
{
    public interface IJwtTokenService
    {
        Task<TokenDTO> GenerateTokenAsync(IdentityUser user);
        Task<TokenDTO> RenewTokenAsync(TokenDTO token, string userId);
    }
}
