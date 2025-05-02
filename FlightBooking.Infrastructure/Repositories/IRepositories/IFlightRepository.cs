using FlightBooking.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightBooking.Infrastructure.Repositories.IRepositories
{
    public interface IFlightRepository
    {
        Task<IEnumerable<ChuyenBay>> GetAllAsync(int page, int pageSize);
        Task<IEnumerable<ChuyenBay>> SearchFlightsAsync(string dDen, string dDi, DateOnly? timeKhoiHanh, int page, int pageSize);
        Task<bool> CreateAsync(ChuyenBay model);
        Task<bool> DeleteAsync(int id);
        Task<bool> UpdateAsync(ChuyenBay model);
    }
}
