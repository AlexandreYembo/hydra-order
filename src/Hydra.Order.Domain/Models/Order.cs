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
        
        public void AddItem(OrderItem orderItem)
        {
            if(_orderItems.Any(a => a.ProductId == orderItem.ProductId))
            {
                var existingItem = _orderItems.FirstOrDefault( p => p.ProductId == orderItem.ProductId);
                existingItem.AddQty(orderItem.Qty);
                orderItem = existingItem;
                _orderItems.Remove(existingItem);
            }

           _orderItems.Add(orderItem);

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