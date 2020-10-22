using System.Threading.Tasks;
using Hydra.Order.Domain.Vouchers;
using Microsoft.EntityFrameworkCore;

namespace Hydra.Order.Data.Repositories
{
    public class VoucherRepository : IVoucherRepository
    {
        private readonly OrderContext _context;

        public VoucherRepository(OrderContext context)
        {
            _context = context;
        }
        public async Task<Voucher> GetVoucherByCode(string code)
        {
            return await _context.Vourcher.AsNoTracking().FirstOrDefaultAsync(f => f.Code == code);
        }

        public void Update(Voucher voucher)
        {
            _context.Vourcher.Update(voucher);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}