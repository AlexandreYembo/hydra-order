using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation.Results;
using Hydra.Core.DomainObjects;
using Hydra.Order.Domain.Enumerables;
using Hydra.Order.Domain.Vouchers;

namespace Hydra.Order.Domain.Orders
{
    /// <summary>
    /// IAggregateRoot is to mark that this entity is part of the aggregate root
    /// </summary>
    public class Order : Entity, IAggregateRoot
    {
        public static int MAX_QTY_PER_ITEM => 15;
        public static int MIN_QTY_PER_ITEM => 1;
        
        //EF Require a parameterless constructor
        protected Order(){
            _orderItems= new List<OrderItem>();
        }

        public Order(Guid customerId, decimal amount, List<OrderItem> orderItems, bool isVoucherUsed = false, decimal discount = 0, Guid? voucherId = null)
        {
            CustomerId = customerId;
            Amount = amount;
            CreatedDate = DateTime.Now;
            _orderItems = orderItems;

            DiscountApplied = discount;
            IsUsedVoucher = isVoucherUsed;
            VourcherId = voucherId;
        }

        public int Code { get; private set; }
        public decimal Amount { get; private set; }

        public decimal DiscountApplied {get; private set;}

        public Guid CustomerId { get; private set; }

        public OrderStatus OrderStatus { get; private set; }

        public Guid? VourcherId { get; private set; }
        
        public bool IsUsedVoucher { get; private set; }

        public DateTime CreatedDate { get; private set; }

      
        private readonly List<OrderItem> _orderItems;

        public void SetAddress(Address address) => Address = address;

        public IReadOnlyCollection<OrderItem> OrderItems  => _orderItems;

        public Address Address { get; private set; }

        //EF relationship
        public Voucher Voucher { get; private set; }

        public void MakeDraft() => OrderStatus = OrderStatus.Draft;
        public void StartOrder() => OrderStatus = OrderStatus.Started;
        public void ProcessingOrder() => OrderStatus = OrderStatus.Processing;
        public void ConfirmOrder() => OrderStatus = OrderStatus.Processed;
        public void DeliverOrder() => OrderStatus = OrderStatus.Delivered;
        public void RefuseOrder() => OrderStatus = OrderStatus.Refused;
        public void CancelOrder() => OrderStatus = OrderStatus.Cancelled;

        public void ApplyVoucher(Voucher voucher)
        {
            var validationResult =  voucher.VoucherIsApplicable();
            if(!validationResult) return;

            Voucher = voucher;
            IsUsedVoucher = true;
            VourcherId = voucher.Id;
        }

        public void CalculateOrderAmount(){
            Amount = OrderItems.Sum(i => i.CalculateAmount());
           CalculateTotalDiscountAmount();
        } 

        public void CalculateTotalDiscountAmount(){
            if(!IsUsedVoucher) return;

            decimal discount = 0;
            var price = Amount;

            if(Voucher.VoucherType == VoucherType.Value)
            {
                if(Voucher.DiscountAmount.HasValue){
                    discount = Voucher.DiscountAmount.Value;
                    price -= discount;
                }
            }
            else
            {
                if(Voucher.DiscountPercentage.HasValue)
                {
                    discount = (Amount * Voucher.DiscountPercentage).Value / 100;
                    price -= discount;
                }
            }

            Amount = price < 0 ? 0 : price;
            DiscountApplied = discount;
        }
        
        public bool HasOrderItem(OrderItem orderItem) => _orderItems.Any(a => a.ProductId == orderItem.ProductId);

        private void ExistingItemValidate(OrderItem item)
        {
            if(!HasOrderItem(item)) throw new DomainException("Item does not exist");
        }

        private void ValidItemQtyAllowed(OrderItem item)
        {
            var qtyItems = item.Qty;
            if(HasOrderItem(item))
            {
                var existingItem = _orderItems.FirstOrDefault( p => p.ProductId == item.ProductId);
                qtyItems += existingItem.Qty;
            }

            if(qtyItems > MAX_QTY_PER_ITEM) throw new DomainException($"Maximum {MAX_QTY_PER_ITEM} per item");
        }

        public void AddItem(OrderItem orderItem)
        {
            ValidItemQtyAllowed(orderItem);
            
            orderItem.AddOrder(Id);
            
            if(HasOrderItem(orderItem))
            {
                var existingItem = _orderItems.FirstOrDefault( p => p.ProductId == orderItem.ProductId);

                existingItem.AddQty(orderItem.Qty);
                orderItem = existingItem;
                _orderItems.Remove(existingItem);
            }

            orderItem.CalculateAmount();

           _orderItems.Add(orderItem);

           CalculateOrderAmount();
        }

        public void UpdateQty(OrderItem orderItem, int qty)
        {
            orderItem.UpdateQty(qty);
            UpdateItem(orderItem);
        }

        public void UpdateItem(OrderItem orderItem)
        {
            if(!orderItem.IsValid().IsValid) return;
            orderItem.AddOrder(Id);

            ExistingItemValidate(orderItem);
            ValidItemQtyAllowed(orderItem);

            var existingItem = OrderItems.FirstOrDefault(p => p.ProductId == orderItem.ProductId);
            _orderItems.Remove(existingItem);
            _orderItems.Add(orderItem);

            CalculateOrderAmount();
        }

        public void RemoveItem(OrderItem orderItem)
        {
            if(!orderItem.IsValid().IsValid) return;

            ExistingItemValidate(orderItem);

            _orderItems.Remove(orderItem);

            CalculateOrderAmount();
        }

        public static class OrderFactory
        {
            public static Orders.Order NewOrderDraft(Guid customerId)
            {
                var order = new Order{
                    CustomerId = customerId
                };

                order.MakeDraft();

                return order;
            }
        }
    }
}