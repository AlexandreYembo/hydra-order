using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation.Results;
using Hydra.Core.Communication.Mediator;
using Hydra.Core.Integration.Messages;
using Hydra.Core.Integration.Messages.OrderMessages;
using Hydra.Core.MessageBus;
using Hydra.Core.Messages;
using Hydra.Core.Messages.CommonMessages.Notifications;
using Hydra.Order.API.Application.Commands.OrderCommands;
using Hydra.Order.API.Application.DTO;
using Hydra.Order.API.Application.Events.OrderEvents;
using Hydra.Order.Domain.Enumerables;
using Hydra.Order.Domain.Orders;
using Hydra.Order.Domain.Repository;
using Hydra.Order.Domain.Vouchers;
using Hydra.Order.Domain.Vouchers.Specs;
using MediatR;

namespace Hydra.Order.API.Application.Commands.Handlers
{
    public class OrderCommandHandler : CommandHandler,
                                    IRequestHandler<CreateOrderCommand, ValidationResult>
                                    // IRequestHandler<StartOrderCommand, ValidationResult>,
                                    // IRequestHandler<ProcessingOrderCommand, ValidationResult>
    {
        private readonly IMediatorHandler _mediatorHandler;
        private readonly IOrderRepository _orderRepository;
        private readonly IMessageBus _messageBus;

        public OrderCommandHandler(IMediatorHandler mediatorHandler, IOrderRepository orderRepository, IMessageBus messageBus)
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
            
            //ApplyVoucher
            ApplyVoucher(message, order);

            order.CalculateOrderAmount();

            if(!await ProcessPayment(order, message.Payment)) return ValidationResult;

            order.AddEvent(new OrderStartedEvent(order.Id, order.CustomerId));

            _orderRepository.AddOrder(order);
            return await Save(_orderRepository.UnitOfWork);
        }

        private bool ApplyVoucher(CreateOrderCommand message, Domain.Orders.Order order)
        {
            if (!message.HasVoucher) return true;
            var voucher = new Voucher(message.Voucher.Code, message.Voucher.Discount, 1, (VoucherType)message.Voucher.DiscountType, DateTime.Now.AddMinutes(1), true, false);

            var voucherValidation = new VoucherValidation().Validate(voucher);
            if (!voucherValidation.IsValid)
            {
                voucherValidation.Errors.ToList().ForEach(m => AddError(m.ErrorMessage));
                return false;
            }

            order.ApplyVoucher(voucher);
            return true;
        }

        public async Task<bool> ProcessPayment(Domain.Orders.Order order, PaymentDTO payment)
        {
            var orderRequest = new OrderInProcessingIntegrationEvent()
            {
                OrderId = order.Id,
                CustomerId = order.CustomerId,
                Price = order.Amount,
                PaymentType = 1, //TODO: Implement the proper type in the future
                CardName = payment.CardHolderName,
                CardNumber = payment.CardNumber,
                Expiration = payment.CardExpiration,
                CVV = payment.CardCvv
            };
            
            var response = await _messageBus.RequestAsync<OrderInProcessingIntegrationEvent, ResponseMessage>(orderRequest);

            if(response.ValidResult.IsValid) return true;

            foreach (var error in response.ValidResult.Errors)
            {
                AddError(error.ErrorMessage);
            }

            return false;
        }


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
                            message.HasVoucher, message.Voucher.Discount);
                            
            order.SetAddress(address);
            order.StartOrder();
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