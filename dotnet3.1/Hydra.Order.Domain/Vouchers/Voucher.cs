using System;
using System.Collections.Generic;
using FluentValidation.Results;
using Hydra.Core.DomainObjects;
using Hydra.Order.Domain.Enumerables;
using Hydra.Order.Domain.Vouchers.Specs;

namespace Hydra.Order.Domain.Vouchers
{
    public class Voucher : Entity, IAggregateRoot
    {
        public Voucher(string code, decimal? discount, int quantity,
            VoucherType voucherType,  DateTime expirationDate, bool active, bool isUsed)
        {
            Code = code;
            Discount = discount;
            Quantity = quantity;
            VoucherType = voucherType;
            ExpirationDate = expirationDate;
            Active = active;
            IsUsed = isUsed;
        }

        public string Code { get; private set; }
        public decimal? Discount { get; private set; }
        public VoucherType VoucherType { get; private set; }
        public int Quantity { get; private set; }
        public DateTime CreatedDate { get; private set; }
        public DateTime UsedDate { get; private set; }
        public DateTime ExpirationDate { get; private set; }
        public bool Active { get; private set; }
        public bool IsUsed { get; private set; }

        // public ICollection<Order> Order { get; set; }

        public bool VoucherIsApplicable() =>
            new VoucherActiveSpecification()
                            .And(new VoucherDataSpecification())
                            .And(new VoucherQuantitySpecification())
                            .IsSatisfiedBy(this);

        internal void MaskAsUsed()
        {
            Active = false;
            IsUsed = true;
            Quantity = 0;
            UsedDate = DateTime.Now;
        }

        public void RemoveQuantity()
        {
            Quantity -= 1;
            if(Quantity >= 1) return;

            MaskAsUsed();
        }
    }
}