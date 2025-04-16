using FlightBooking.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightBooking.Infrastructure.Repositories.IRepositories
{
    public interface IFlightRouteRepository
    {
        public Task<bool> AddAsync(TuyenBay model);
        public Task<bool> UpdateAsync(TuyenBay model);
        public Task<bool> DeleteAsync(int id);
        public Task<IEnumerable<TuyenBay>> GetAllAsync();
    }
}
