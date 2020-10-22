using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation.Results;
using Hydra.Core.Communication.Mediator;
using Hydra.Core.Integration.Messages;
using Hydra.Core.MessageBus;
using Hydra.Core.Messages;
using Hydra.Core.Messages.CommonMessages.Notifications;
using Hydra.Order.API.Application.Commands.OrderCommands;
using Hydra.Order.API.Application.Commands.VoucherCommands;
using Hydra.Order.API.Application.DTO;
using Hydra.Order.API.Application.Events.OrderEvents;
using Hydra.Order.API.Application.Events.VoucherEvents;
using Hydra.Order.Domain.Orders;
using Hydra.Order.Domain.Repository;
using Hydra.Order.Domain.Vouchers;
using Hydra.Order.Domain.Vouchers.Specs;
using MediatR;

namespace Hydra.Order.API.Application.Commands.Handlers
{
    public class OrderCommandHandler : CommandHandler,
                                    IRequestHandler<CreateOrderCommand, ValidationResult>,
                                    IRequestHandler<ApplyVoucherOrderCommand, ValidationResult>,
                                    IRequestHandler<StartOrderCommand, ValidationResult>
    {
        private readonly IMediatorHandler _mediatorHandler;
        private readonly IOrderRepository _orderRepository;
        private readonly IMessageBus _messageBus;

        public OrderCommandHandler(IMediatorHandler mediatorHandler, IOrderRepository orderRepository,
                                    IMessageBus messageBus)
        {
            _mediatorHandler = mediatorHandler;
            _orderRepository = orderRepository;
            _messageBus = messageBus;
        }

        public async Task<ValidationResult> Handle(CreateOrderCommand message, CancellationToken cancellationToken)
        {
            //Command Validation
            if(!message.IsValid()) return message.ValidationResult;

            //Order Mapper
            var order = OrderMap(message);

            _orderRepository.AddOrder(order);
            var result = await Save(_orderRepository.UnitOfWork);

            if(!result.IsValid) return ValidationResult;

            //Publish the event to start the order
            await _mediatorHandler.PublishEvent(new OrderDraftStartedEvent(message.CustomerId, message.VoucherCode, order.Id, order.IsUsedVoucher));
            //Order Validation
            return ValidationResult;
        }


        /// <summary>
        /// Workflow to apply the voucher
        /// </summary>
        /// <param name="message"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<ValidationResult> Handle(ApplyVoucherOrderCommand message, CancellationToken cancellationToken)
        {
            if(!message.VoucherApplied) {
                await _mediatorHandler.PublishEvent(new OrderWithoutVoucherVerifiedEvent(message.OrderId, message.CustomerId));
                return message.ValidationResult;
            }

            //TODO: Implement when the Voucher Api will be developed.
    
            if(1 == 2){ //Remove this later
                var voucherRequest = new VoucherIntegrationEvent(message.Code);
                var rrrr = await _messageBus.RequestAsync<VoucherIntegrationEvent, ResponseMessage>(voucherRequest);
                return message.ValidationResult;
            }

            var voucher = new Voucher("213123", 1, 1, 1, 0, DateTime.Now.AddDays(2), true, false);// TODO: Remove

            if(voucher == null)
            {
                AddError("Voucher does not exist");
                await _mediatorHandler.PublishEvent(new VoucherRefusedEvent(message.CustomerId, message.OrderId, message.Code));
                return message.ValidationResult;
            }

            var voucherValidation = new VoucherValidation().Validate(voucher);

            if(!voucherValidation.IsValid)
            {
                voucherValidation.Errors.ToList().ForEach(v => AddError(v.ErrorCode));
                //await _mediatorHandler.PublishEvent(new VoucherRefusedEvent(message.CustomerId, order.Id, message.VoucherCode));
                return message.ValidationResult;
            }

            var order = await _orderRepository.GetOrderById(message.OrderId);
            
            order.ApplyVoucher(voucher);
            
            voucher.RemoveQuantity();

            _orderRepository.UpdateOrder(order);
            var result  = await Save(_orderRepository.UnitOfWork);
            
            if(result.IsValid)
                await _mediatorHandler.PublishEvent(new VoucherAppliedEvent(order.Id, order.CustomerId, voucher.Id));

            return ValidationResult;
        }

        public async Task<ValidationResult> Handle(StartOrderCommand request, CancellationToken cancellationToken)
        {
            var order = await _orderRepository.GetOrderById(request.OrderId);
            
            return null;
//            order.SetAddress();

        }

        private async Task NotifyOrderNotFound() => 
            await _mediatorHandler.PublishNotification(new DomainNotification("Order", "Order not found"));

        private Domain.Orders.Order OrderMap(CreateOrderCommand message)
        {
            var address = new Address
            {
                Street = message.Address.Street,
                Number = message.Address.Number,
                City = message.Address.City,
                State = message.Address.State,
                Country = message.Address.Country,
                PostCode = message.Address.PostCode
            };

            var order = new Domain.Orders.Order(message.CustomerId, message.TotalPrice, message.Items.Select(OrderItemDTO.Map).ToList(),
                            message.HasVoucher, message.Discount);
                            
            order.SetAddress(address);
            order.MakeDraft();
            return order;
        }

        private bool IsValidCommand(Command message)
        {
            if(message.IsValid()) return true;

            foreach (var error in message.ValidationResult.Errors)
            {
                _mediatorHandler.PublishNotification(new DomainNotification(message.MessageType, error.ErrorMessage));
            }
            return false;
        }

        private bool OrderValidation(Domain.Orders.Order order)
        {
            var orderPrice = order.Amount;
            var orderDiscount = order.DiscountApplied;

            order.CalculateOrderAmount();

            if(order.Amount != orderPrice)
            {
                AddError("Price of order is different");
                return false;
            }

            if(order.DiscountApplied != orderDiscount)
            {
                AddError("Discount Applied is different");
                return false;
            }

            return true;
        }
    }
}