using System;
using System.Threading;
using System.Threading.Tasks;
using Hydra.Order.Application.Commands;
using Hydra.Order.Application.Queries;
using Hydra.Order.Domain.Repository;
using MediatR;
using Moq;
using Moq.AutoMock;
using Xunit;

namespace Hydra.Order.Application.Tests.Orders
{
    public class OrderCommandHandlerTests
    {
        private readonly Guid _customerId;
        private readonly Guid _productId;
        private readonly Domain.Models.Order _order;

        private readonly AutoMocker _mocker;
        private readonly OrderCommandHandler _orderHandler;

        public OrderCommandHandlerTests()
        {
            _mocker = new AutoMocker();
            _orderHandler = _mocker.CreateInstance<OrderCommandHandler>();

            _customerId = Guid.NewGuid();
            _productId = Guid.NewGuid();

            _order = Domain.Models.Order.OrderFactory.NewOrderDraft(_customerId); 
        }


        [Fact(DisplayName="Add item New Order Success")]
        [Trait("Order", "Order Comand Handler")]
        public async Task AddItem_NewOrder_ShouldExectureWithSucess()
        {
            //Arrange
            var orderCommand = new AddOrderItemCommand(_customerId, _productId, "Product A", 2, 100);

            _mocker.GetMock<IOrderRepository>().Setup(r => r.UnitOfWork.Commit()).Returns(Task.FromResult(true));

            //Act
            var result = await _orderHandler.Handle(orderCommand, CancellationToken.None);
            
            //Assert
            Assert.True(result);
            _mocker.GetMock<IOrderRepository>().Verify(r => r.AddOrder(It.IsAny<Domain.Models.Order>()), Times.Once);
            _mocker.GetMock<IOrderRepository>().Verify(r => r.UnitOfWork.Commit(), Times.Once);
        }

        [Fact(DisplayName="Add item Draft Order Success")]
        [Trait("Order", "Order Comand Handler")]
        public async Task AddItem_NewItemDraftOrder_ShouldExecuteWithSucess()
        {
            //Arrange

            var existingItem = new Domain.Models.OrderItem(Guid.NewGuid(), "Test Product A", 2, 100);
            _order.AddItem(existingItem);
            
            var orderCommand = new AddOrderItemCommand(_customerId, Guid.NewGuid(), "Product A", 2, 100);

            _mocker.GetMock<IOrderRepository>()
                .Setup(r => r.GetOrderDraftByCustomerId(_customerId))
                .Returns(Task.FromResult(_order));
            _mocker.GetMock<IOrderRepository>().Setup(r => r.UnitOfWork.Commit()).Returns(Task.FromResult(true));

            //Act
            var result = await _orderHandler.Handle(orderCommand, CancellationToken.None);
            
            //Assert
            Assert.True(result);
            _mocker.GetMock<IOrderRepository>().Verify(r => r.AddOrderItem(It.IsAny<Domain.Models.OrderItem>()), Times.Once);
            _mocker.GetMock<IOrderRepository>().Verify(r => r.UpdateOrder(It.IsAny<Domain.Models.Order>()), Times.Once);
            _mocker.GetMock<IOrderRepository>().Verify(r => r.UnitOfWork.Commit(), Times.Once);
        }

        [Fact(DisplayName="Add Existing item Draft Order Success")]
        [Trait("Order", "Order Comand Handler")]
        public async Task AddItem_ExistingItemDraftOrder_ShouldExecuteWithSucess()
        {
            //Arrange
           
            var existingItem = new Domain.Models.OrderItem(_productId, "Test Product A", 2, 100);
            _order.AddItem(existingItem);
            
            var orderCommand = new AddOrderItemCommand(_customerId, _productId, "Product A", 2, 100);

            _mocker.GetMock<IOrderRepository>()
                .Setup(r => r.GetOrderDraftByCustomerId(_customerId))
                .Returns(Task.FromResult(_order));
            _mocker.GetMock<IOrderRepository>().Setup(r => r.UnitOfWork.Commit()).Returns(Task.FromResult(true));

            //Act
            var result = await _orderHandler.Handle(orderCommand, CancellationToken.None);
            
            //Assert
            Assert.True(result);
            _mocker.GetMock<IOrderRepository>().Verify(r => r.UpdateOrderItem(It.IsAny<Domain.Models.OrderItem>()), Times.Once);
            _mocker.GetMock<IOrderRepository>().Verify(r => r.UpdateOrder(It.IsAny<Domain.Models.Order>()), Times.Once);
            _mocker.GetMock<IOrderRepository>().Verify(r => r.UnitOfWork.Commit(), Times.Once);
        }

        [Fact(DisplayName="Add item Invalid Command")]
        [Trait("Order", "Order Comand Handler")]
        public async Task AddItem_InvalidCommand_ShouldReturnFalseAndSendNotificationEvent()
        {
            //Arrange
            var orderCommand = new AddOrderItemCommand(Guid.Empty, Guid.Empty, string.Empty, 0, 0);
            
            //Act
            var result = await _orderHandler.Handle(orderCommand, CancellationToken.None);
         
             //Assert
            Assert.False(result);

            //Times = 5, because there are 5 itens on the constructor that are invalid and I will check how many times the notifications were sent.
            _mocker.GetMock<IMediator>().Verify(m => m.Publish(It.IsAny<INotification>(), CancellationToken.None), Times.Exactly(5));
        }
    }
}