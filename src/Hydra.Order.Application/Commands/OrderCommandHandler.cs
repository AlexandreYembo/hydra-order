using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Hydra.Core.Communication.Mediator;
using Hydra.Core.Messages;
using Hydra.Core.Messages.CommonMessages.Notifications;
using Hydra.Order.Application.Events;
using Hydra.Order.Domain.Models;
using Hydra.Order.Domain.Repository;
using MediatR;

namespace Hydra.Order.Application.Commands
{
    public class OrderCommandHandler : CommandHandler, 
                                       IRequestHandler<AddOrderItemCommand, bool>,
                                       IRequestHandler<UpdateOrderItemCommand, bool>,
                                       IRequestHandler<RemoveOrderItemCommand, bool>,
                                       IRequestHandler<ApplyVoucherOrderCommand, bool>

    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMediatorHandler _mediatorHandler;

        public OrderCommandHandler(IOrderRepository orderRepository, IMediatorHandler mediatorHandler) : base(mediatorHandler)
        {
            _orderRepository = orderRepository;
            _mediatorHandler = mediatorHandler;
        }

        public async Task<bool> Handle(AddOrderItemCommand message, CancellationToken cancellationToken)
        {
            
            if(!IsCommandValid(message)) return false;
            
            var order = await _orderRepository.GetOrderDraftByCustomerId(message.CustomerId);
            var orderItem = new OrderItem(message.ProductId, message.ProductName, message.Qty, message.Price);

            if(order == null)
            {
                order = AddOrder(message, orderItem);
                order.AddEvent(new OrderDraftStartedEvent(order.CustomerId, order.Id));
            }
            else
            {
                UpdateExistingOrder(order, orderItem);
                order.AddEvent(new OrderUpdatedEvent(order.CustomerId, order.Id, order.Amount));
            }

            order.AddEvent(new OrderItemAddedEvent(order.CustomerId, order.Id, message.ProductId, message.ProductName, message.Qty, message.Price));

            return await _orderRepository.UnitOfWork.Commit();
        }

        public async Task<bool> Handle(UpdateOrderItemCommand message, CancellationToken cancellationToken)
        {
            if(!IsCommandValid(message)) return false;

            var order = await _orderRepository.GetOrderDraftByCustomerId(message.CustomerId);

            if(order == null)
            {
                await NotifyOrderNotFounded();
                return false;
            }

            var orderItem = await _orderRepository.GetOrderItemByOrder(order.Id, message.ProductId);

            if(!order.HasOrderItem(orderItem))
            {
                await _mediatorHandler.PublishNotification(new DomainNotification("order", "Order item not founded!"));
                return false;
            }

            order.UpdateQty(orderItem, message.Qty);

            order.AddEvent(new OrderUpdatedEvent(order.CustomerId, order.Id, order.Amount));
            order.AddEvent(new OrderItemUpdatedEvent(order.CustomerId, order.Id, message.ProductId, message.Qty));

            _orderRepository.UpdateOrderItem(orderItem);
            _orderRepository.UpdateOrder(order);

            return await _orderRepository.UnitOfWork.Commit();
        }

        private async Task NotifyOrderNotFounded()
        {
            await _mediatorHandler.PublishNotification(new DomainNotification("order", "order not founded!"));
        }

        public async Task<bool> Handle(RemoveOrderItemCommand message, CancellationToken cancellationToken)
        {
             if(!IsCommandValid(message)) return false;

            var order = await _orderRepository.GetOrderDraftByCustomerId(message.CustomerId);

            if(order == null){
                await NotifyOrderNotFounded();
                return false;
            }

            var orderItem = await _orderRepository.GetOrderItemByOrder(order.Id, message.ProductId);

            if(!order.HasOrderItem(orderItem))
            {
                await _mediatorHandler.PublishNotification(new DomainNotification("order", "Order item not founded!"));
                return false;
            }

            order.RemoveItem(orderItem);

            order.AddEvent(new OrderUpdatedEvent(order.CustomerId, order.Id, order.Amount));
            order.AddEvent(new OrderItemRemovedEvent(order.CustomerId, order.Id, message.ProductId));

            _orderRepository.UpdateOrderItem(orderItem);
            _orderRepository.UpdateOrder(order);

            return await _orderRepository.UnitOfWork.Commit();
        }

        public async Task<bool> Handle(ApplyVoucherOrderCommand message, CancellationToken cancellationToken)
        {
            if(!IsCommandValid(message)) return false;

            var order = await _orderRepository.GetOrderDraftByCustomerId(message.CustomerId);

            if(order == null)
            {
                await NotifyOrderNotFounded();
                return false;
            }

            var voucher = await _orderRepository.GetVoucherByCode(message.VoucherCode);

            if(voucher == null){
                await _mediatorHandler.PublishNotification(new DomainNotification("order", "Voucher not founded!"));
                return false;
            }

            var voucherAplicableValidation = order.ApplyVoucher(voucher);

            if(!voucherAplicableValidation.IsValid){
                foreach (var error in voucherAplicableValidation.Errors)
                {
                    await _mediatorHandler.PublishNotification(new DomainNotification(error.ErrorCode, error.ErrorMessage));
                }
                return false;
            }

            order.AddEvent(new OrderUpdatedEvent(order.CustomerId, order.Id, order.Amount));
            order.AddEvent(new VoucherApplyToOrderEvent(message.CustomerId, order.Id, voucher.Id));

            _orderRepository.UpdateOrder(order);

            return await _orderRepository.UnitOfWork.Commit();
        }

        private Domain.Models.Order AddOrder(AddOrderItemCommand message, OrderItem orderItem)
        {
            Domain.Models.Order order = Domain.Models.Order.OrderFactory.NewOrderDraft(message.CustomerId);
            order.AddItem(orderItem);
            _orderRepository.AddOrder(order);
            return order;
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

        public bool IsCommandValid(Command message)
        {
            if(message.IsValid()) return true;

            foreach (var error in message.ValidationResult.Errors)
            {
                //MessageType -> name of the class
                _mediatorHandler.PublishNotification(new DomainNotification(message.MessageType, error.ErrorMessage));
            }

            return false;
        }
    }
}