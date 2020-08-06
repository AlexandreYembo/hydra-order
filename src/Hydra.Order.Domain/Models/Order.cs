using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation.Results;
using Hydra.Core.DomainObjects;
using Hydra.Order.Domain.Enumerables;

namespace Hydra.Order.Domain.Models
{
    /// <summary>
    /// IAggregateRoot is to mark that this entity is part of the aggregate root
    /// </summary>
    public class Order : Entity, IAggregateRoot
    {
        public static int MAX_QTY_PER_ITEM => 15;
        public static int MIN_QTY_PER_ITEM => 1;
        protected Order(){
            _orderItems= new List<OrderItem>();
        }

        public Order(Guid customerId, bool isUsedVoucher, decimal discount, decimal amount)
        {
            CustomerId = customerId;
            IsUsedVoucher = isUsedVoucher;
            DiscountApplied = discount;
            Amount = amount;
            _orderItems = new List<OrderItem>();
        }

        public int Code { get; private set; }
        public decimal Amount { get; private set; }

        public decimal DiscountApplied {get; private set;}

        public Guid CustomerId { get; private set; }

        public OrderStatus OrderStatus { get; private set; }

        public Guid? VourcherId { get; private set; }
        
        public bool HasVoucher { get; private set; }
        public bool IsUsedVoucher { get; private set; }

        //EF relationship
        public Voucher Voucher { get; private set; }

        private readonly List<OrderItem> _orderItems;
        public IReadOnlyCollection<OrderItem> OrderItems  => _orderItems;

        private void CalculateOrderAmount(){
            Amount = OrderItems.Sum(i => i.CalculateAmount());

            CalculateTotalDiscountAmount();
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

        public ValidationResult ApplyVoucher(Voucher voucher)
        {
            var result =  voucher.IsValid();
            if(!result.IsValid) return result;

            Voucher = voucher;
            HasVoucher = true;

            CalculateTotalDiscountAmount();

            return result;
        }

        public void CalculateTotalDiscountAmount(){
            if(!HasVoucher) return;

            decimal discount = 0;
            var price = Amount;

            if(Voucher.VoucherType == VoucherType.Value)
            {
                if(Voucher.DiscountAmount.HasValue){
                    discount = Voucher.DiscountAmount.Value;
                    price -= discount;
                }
            }
            else{
                if(Voucher.DiscountPercentage.HasValue)
                {
                    discount = (Amount * Voucher.DiscountPercentage).Value / 100;
                    price -= discount;
                }
            }

            Amount = price < 0 ? 0 : price;
            DiscountApplied = discount;
        }

        public void MakeDraft() => OrderStatus = OrderStatus.Draft;
        public void StartOrder() => OrderStatus = OrderStatus.Started;
        public void ConfirmOrder() => OrderStatus = OrderStatus.Processed;
        public void DeliverOrder() => OrderStatus = OrderStatus.Delivered;
        public void CancelOrder() => OrderStatus = OrderStatus.Cancelled;


        public static class OrderFactory
        {
            public static Models.Order NewOrderDraft(Guid customerId)
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