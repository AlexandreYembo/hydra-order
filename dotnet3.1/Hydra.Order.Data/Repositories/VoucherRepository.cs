using System.Threading.Tasks;
using Hydra.Core.Data;
using Hydra.Order.Domain.Models;
using Hydra.Order.Domain.Repository;
using Microsoft.EntityFrameworkCore;

namespace Hydra.Order.Data.Repositories
{
    public class VoucherRepository : IVoucherRepository
    {
        private readonly OrderContext _context;
        public async Task<Voucher> GetVoucherByCode(string code)
        {
            return await _context.Vourcher.AsNoTracking().FirstOrDefaultAsync(f => f.Code == code);
        }

        public void Dispose()
        {
            _context.Dispose();
        }

    }
}