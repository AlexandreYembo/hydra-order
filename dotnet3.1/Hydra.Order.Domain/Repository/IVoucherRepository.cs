using System.Threading.Tasks;
using Hydra.Core.Data;
using Hydra.Order.Domain.Models;

namespace Hydra.Order.Domain.Repository
{
    public interface IVoucherRepository
    {
        Task<Voucher> GetVoucherByCode(string code);
    }
}