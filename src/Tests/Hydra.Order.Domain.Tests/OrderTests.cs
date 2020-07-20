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
        [Trait("Order", "Order")]
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
        [Trait("Order", "Order")]
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

        [Fact(DisplayName= "New Item order with quantity Greater than allowed")]
        [Trait("Order", "Order")]
        public void AddOrderItem_ItemGreaterThanAllowedQty_ShouldReturnException()
        {
            //Arrange
            var order = Models.Order.OrderFactory.NewOrderDraft(Guid.NewGuid());
            var orderItem = new OrderItem(Guid.NewGuid(), "Test Product", Models.Order.MAX_QTY_PER_ITEM + 1, 100);

            // Act && Assert
            Assert.Throws<DomainException>(() =>  order.AddItem(orderItem));
        }

        [Fact(DisplayName= "Existing Item order with quantity Greater than allowed")]
        [Trait("Order", "Order")]
        public void AddOrderItem_ExistingItemGreaterThanAllowedQty_ShouldReturnException()
        {
            //Arrange
             //Arrange
            var order = Models.Order.OrderFactory.NewOrderDraft(Guid.NewGuid());

            var productId = Guid.NewGuid();

            var orderItem = new OrderItem(productId, "Test Product", 1, 100);
            order.AddItem(orderItem);

            var orderItem2 = new OrderItem(productId, "Test Product", Models.Order.MAX_QTY_PER_ITEM, 100);

            // Act && Assert
            Assert.Throws<DomainException>(() =>  order.AddItem(orderItem2));
        }

        [Fact(DisplayName= "Update item existing order")]
        [Trait("Order", "Order")]
        public void UpdateOrderItem_ItemDoesNotExist_ShouldReturnException()
        {
            //Arrange
            var order = Models.Order.OrderFactory.NewOrderDraft(Guid.NewGuid());
            var orderItem = new OrderItem(Guid.NewGuid(), "Test Product", Models.Order.MAX_QTY_PER_ITEM + 1, 100);

            // Act && Assert
            Assert.Throws<DomainException>(() =>  order.UpdateItem(orderItem));
        }

        [Fact(DisplayName= "Existing Valid Item")]
        [Trait("Order", "Order")]
        public void ExistingOrderItem_ValidItem_ShouldAllowToUpdateQty()
        {
            //Arrange
             //Arrange
            var order = Models.Order.OrderFactory.NewOrderDraft(Guid.NewGuid());

            var productId = Guid.NewGuid();

            var orderItem = new OrderItem(productId, "Test Product", 1, 100);
            order.AddItem(orderItem);

            var orderItemUpdated = new OrderItem(productId, "Test Product", 5, 100);
            var newQty = orderItemUpdated.Qty;

            //Act
            order.UpdateItem(orderItemUpdated);

            //Assert
            Assert.Equal(newQty, order.OrderItems.FirstOrDefault(p => p.ProductId == productId).Qty);
        }

        [Fact(DisplayName= "Existing Valid Item Validate Amount")]
        [Trait("Order", "Order")]
        public void ExistingOrderItem_DifferentItems_ShouldAllowToUpdateAmount()
        {
            //Arrange
             //Arrange
            var order = Models.Order.OrderFactory.NewOrderDraft(Guid.NewGuid());

            var productId1 = Guid.NewGuid();
            var productId2 = Guid.NewGuid();

            var orderItem1 = new OrderItem(productId1, "Test Product A", 2, 100);
            var orderItem2 = new OrderItem(productId2, "Test Product B", 3, 15);
            order.AddItem(orderItem1);
            order.AddItem(orderItem2);

            var orderItemUpdated = new OrderItem(productId2, "Test Product B", 5, 15);
            var amountItems = orderItem1.Qty * orderItem1.Price + orderItemUpdated.Qty * orderItemUpdated.Price;

            //Act
            order.UpdateItem(orderItemUpdated);

            //Assert
            Assert.Equal(amountItems, order.Amount);
        }

        [Fact(DisplayName= "Update item qty greater than allowd")]
        [Trait("Order", "Order")]
        public void UpdateOrderItem_ItemQtyGreaterThanAllowed_ShouldReturnException()
        {
            //Arrange
            var order = Models.Order.OrderFactory.NewOrderDraft(Guid.NewGuid());

            var productId = Guid.NewGuid();

            var orderItem = new OrderItem(Guid.NewGuid(), "Test Product", 3, 15);
            order.AddItem(orderItem);

            var orderItemUpdated = new OrderItem(Guid.NewGuid(), "Test Product", Models.Order.MAX_QTY_PER_ITEM + 1, 100);

            // Act && Assert
            Assert.Throws<DomainException>(() =>  order.UpdateItem(orderItemUpdated));
        }

        [Fact(DisplayName= "Update item qty less than allowd")]
        [Trait("Order", "Order")]
        public void UpdateOrderItem_ItemQtyLessThanAllowed_ShouldReturnException()
        {
            //Arrange
            var order = Models.Order.OrderFactory.NewOrderDraft(Guid.NewGuid());

            var productId = Guid.NewGuid();

            var orderItem = new OrderItem(Guid.NewGuid(), "Test Product", 3, 15);
            order.AddItem(orderItem);

            // Act && Assert
            Assert.Throws<DomainException>(() =>  new OrderItem(Guid.NewGuid(), "Test Product", Models.Order.MIN_QTY_PER_ITEM -1, 100));
        }
    }
}
