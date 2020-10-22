using System.Threading.Tasks;
namespace Hydra.Order.Domain.Vouchers
{
    public interface IVoucherRepository
    {
        Task<Voucher> GetVoucherByCode(string code);
        void Update(Voucher voucher);
    }
}