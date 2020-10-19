using System.Threading.Tasks;
using Hydra.Order.Application.Models;
using Hydra.Order.Domain.Repository;

namespace Hydra.Order.Application.Queries
{
    public class VoucherQueries : IVoucherQueries
    {
        private readonly IVoucherRepository _voucherRepository;

        public VoucherQueries(IVoucherRepository voucherRepository)
        {
            _voucherRepository = voucherRepository;
        }

        public async Task<VoucherDTO> GetVoucherByCode(string code)
        {
            var voucher = await _voucherRepository.GetVoucherByCode(code);

            if(voucher == null) return null;

            //Validade if the voucher is valid

            return new VoucherDTO   //TODO: Implement automapper
            {
                Code = voucher.Code,
                DiscountType = (int) voucher.VoucherType,
                Percentage = voucher.DiscountPercentage,
                Discount = voucher.DiscountAmount
            };
        }
    }
}