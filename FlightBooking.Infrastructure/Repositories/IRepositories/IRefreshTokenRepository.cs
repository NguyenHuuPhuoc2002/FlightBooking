using FlightBooking.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightBooking.Infrastructure.Repositories.IRepositories
{
    public interface IRefreshTokenRepository
    {
        Task<bool> AddAsync(RefreshToken token);
        Task<bool> UpdateAsync(RefreshToken token, string refreshToken);
        Task<RefreshToken> GetTokenAsync(string refreshToken);
    }
}
