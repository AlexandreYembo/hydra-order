using System.Threading.Tasks;
using Hydra.Order.Application.Models;

namespace Hydra.Order.Application.Queries
{
    public interface IVoucherQueries
    {
         Task<VoucherDTO> GetVoucherByCode(string code);
    }
}