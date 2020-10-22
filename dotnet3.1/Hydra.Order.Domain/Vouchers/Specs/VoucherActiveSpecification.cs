using System;
using System.Linq.Expressions;
using Hydra.Core.Specification;

namespace Hydra.Order.Domain.Vouchers.Specs
{
    public class VoucherActiveSpecification : Specification<Voucher>
    {
        public override Expression<Func<Voucher, bool>> ToExpression() => voucher => voucher.Active && !voucher.IsUsed;
    }
}