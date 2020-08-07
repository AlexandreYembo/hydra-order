using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Hydra.Core.Communication.Mediator;
using Hydra.Order.Application.Events;
using Hydra.Order.Domain.Models;
using Hydra.Order.Domain.Repository;
using MediatR;

namespace Hydra.Order.Application.Commands
{
    public class OrderCommandHandler : CommandHandler, IRequestHandler<AddOrderItemCommand, bool>
    {
        private readonly IOrderRepository _orderRepository;

        public OrderCommandHandler(IOrderRepository orderRepository, IMediatorHandler mediatorHandler) : base(mediatorHandler)
        {
            _orderRepository = orderRepository;
        }

        public async Task<bool> Handle(AddOrderItemCommand message, CancellationToken cancellationToken)
        {
            
            if(!IsCommandValid(message)) return false;
            
            var order = await _orderRepository.GetOrderDraftByCustomerId(message.CustomerId);
            var orderItem = new OrderItem(message.ProductId, message.ProductName, message.Qty, message.Price);

            if(order == null)
            {
                order = AddOrder(message, orderItem);
            }
            else
            {
                UpdateExistingOrder(order, orderItem);
            }

            order.AddEvent(new OrderItemAddedEvent(order.CustomerId, order.Id, message.ProductId, message.ProductName, message.Qty, message.Price));
            
            return await _orderRepository.UnitOfWork.Commit();
        }


        private void UpdateExistingOrder(Domain.Models.Order order, OrderItem orderItem)
        {
            var existingItem = order.HasOrderItem(orderItem);
            order.AddItem(orderItem);

            if(existingItem)
            {
                _orderRepository.UpdateOrderItem(order.OrderItems.FirstOrDefault(p => p.ProductId == orderItem.ProductId));
            }
            else
            {
                _orderRepository.AddOrderItem(orderItem);
            }

            _orderRepository.UpdateOrder(order);
        }

        private Domain.Models.Order AddOrder(AddOrderItemCommand message, OrderItem orderItem)
        {
            Domain.Models.Order order = Domain.Models.Order.OrderFactory.NewOrderDraft(message.CustomerId);
            order.AddItem(orderItem);
            _orderRepository.AddOrder(order);
            return order;
        }
    }
}