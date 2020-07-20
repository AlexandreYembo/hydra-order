using System;
using System.Collections.Generic;
using System.Linq;
using Hydra.Core.DomainObjects;
using Hydra.Order.Domain.Enumerables;

namespace Hydra.Order.Domain.Models
{
    public class Order
    {
        public static int MAX_QTY_PER_ITEM => 15;
        public static int MIN_QTY_PER_ITEM => 1;
        protected Order(){
            _orderItems= new List<OrderItem>();
        }
        public decimal Amount { get; private set; }

        public Guid CustomerId { get; private set; }

        public OrderStatus OrderStatus { get; private set; }

        private readonly List<OrderItem> _orderItems;
        public IReadOnlyCollection<OrderItem> OrderItems  => _orderItems;


        private void CalculateOrderAmount() => Amount = OrderItems.Sum(i => i.CalculateAmount());
        
        private bool HasOrderItem(OrderItem orderItem) => _orderItems.Any(a => a.ProductId == orderItem.ProductId);

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

            if(HasOrderItem(orderItem))
            {
                var existingItem = _orderItems.FirstOrDefault( p => p.ProductId == orderItem.ProductId);

                existingItem.AddQty(orderItem.Qty);
                orderItem = existingItem;
                _orderItems.Remove(existingItem);
            }

           _orderItems.Add(orderItem);

           CalculateOrderAmount();
        }

        public void UpdateItem(OrderItem orderItem)
        {
            ExistingItemValidate(orderItem);
            ValidItemQtyAllowed(orderItem);

            var existingItem = OrderItems.FirstOrDefault(p => p.ProductId == orderItem.ProductId);
            _orderItems.Remove(existingItem);
            _orderItems.Add(orderItem);

            CalculateOrderAmount();
        }

         public void RemoveItem(OrderItem orderItem)
        {
            ExistingItemValidate(orderItem);

            _orderItems.Remove(orderItem);

            CalculateOrderAmount();
        }

        public void MakeDraft() => OrderStatus = OrderStatus.Draft;

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