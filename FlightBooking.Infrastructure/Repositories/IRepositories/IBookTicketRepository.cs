using FlightBooking.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightBooking.Infrastructure.Repositories.IRepositories
{
    public interface IBookTicketRepository
    {
        public Task<IEnumerable<Ve>> Gets();
        public Task<bool> BooKing(Ve model);
        public Task<bool> Cancel(Guid id);
        public Task<bool> AddChiTietLH(ChiTietLienHe model);
        public Task<bool> AddDichVu(ChiTietDichVu model);
    }
}
