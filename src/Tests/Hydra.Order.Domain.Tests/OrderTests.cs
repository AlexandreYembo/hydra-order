using System;
using System.Linq;
using Hydra.Core.DomainObjects;
using Hydra.Order.Domain.Models;
using Xunit;

namespace Hydra.Order.Domain.Tests
{
    public class OrderTests
    {
        [Fact(DisplayName= "Add Item new Order")]
        [Trait("Order", "Order Tests")]
        public void AddOrderItem_NewOrder_ShouldUpdateValue()
        {
            //Arrange
            var order = Models.Order.OrderFactory.NewOrderDraft(Guid.NewGuid());
            var orderItem = new OrderItem(Guid.NewGuid(), "Test Product", 2, 100);

            //Act
            order.AddItem(orderItem);

            //Assert
            Assert.Equal(200, order.Amount);
        }

        [Fact(DisplayName= "Add Item existing order")]
        [Trait("Order", "Order Tests")]
        public void AddOrderItem_ExistingItem_ShouldIncrementQtyAndUpdateValue()
        {
            //Arrange
            var order = Models.Order.OrderFactory.NewOrderDraft(Guid.NewGuid());

            var productId = Guid.NewGuid();

            var orderItem = new OrderItem(productId, "Test Product", 2, 100);
            order.AddItem(orderItem);

            var orderItem2 = new OrderItem(productId, "Test Product", 1, 100);

            //Act
            order.AddItem(orderItem2);

            //Assert
            Assert.Equal(300, order.Amount);
            Assert.Equal(1, order.OrderItems.Count);
            Assert.Equal(3, order.OrderItems.FirstOrDefault( p => p.ProductId == productId).Qty);
        }
    }
}
