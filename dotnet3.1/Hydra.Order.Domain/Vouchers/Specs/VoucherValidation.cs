using Hydra.Core.Specification.Validation;

namespace Hydra.Order.Domain.Vouchers.Specs
{
    public class VoucherValidation : SpecValidator<Voucher>
    {
        public VoucherValidation()
        {
            // var dataSpec = new VoucherDataSpecification();
            // var qtySpec = new VoucherQuantitySpecification();
            var activeSpec = new VoucherActiveSpecification();

            // Add("dataSpec", new Rule<Voucher>(dataSpec, "Voucher expired"));
            // Add("qtySpec", new Rule<Voucher>(qtySpec, "Voucher was used"));
            Add("activeSpec", new Rule<Voucher>(activeSpec, "Voucher is not active"));
        }
    }
}