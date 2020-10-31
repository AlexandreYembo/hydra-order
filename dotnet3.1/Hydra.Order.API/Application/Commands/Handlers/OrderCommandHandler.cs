using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation.Results;
using Hydra.Core.Communication.Mediator;
using Hydra.Core.Messages;
using Hydra.Core.Messages.CommonMessages.Notifications;
using Hydra.Order.API.Application.Commands.OrderCommands;
using Hydra.Order.API.Application.DTO;
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

        public OrderCommandHandler(IMediatorHandler mediatorHandler, IOrderRepository orderRepository)
        {
            _mediatorHandler = mediatorHandler;
            _orderRepository = orderRepository;
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

//         public async Task<ValidationResult> Handle(StartOrderCommand message, CancellationToken cancellationToken)
//         {
//             var order = await _orderRepository.GetOrderById(message.OrderId);
//             order.StartOrder();
            
//             _orderRepository.UpdateOrder(order);

//             var result = await Save(_orderRepository.UnitOfWork);

//             if(result.IsValid)
//                 await _mediatorHandler.PublishEvent(new OrderStartedEvent(order.Id, 
//                                                         order.OrderItems.Select(s => s.Id).ToList()));
//             return ValidationResult;
// //            order.SetAddress();

//         }

//         public async Task<ValidationResult> Handle(ProcessingOrderCommand message, CancellationToken cancellationToken)
//         {
//             var order = await _orderRepository.GetOrderById(message.AggregateId);
//             order.ProcessingOrder();

//             _orderRepository.UpdateOrder(order);

//             return await Save(_orderRepository.UnitOfWork);
//         }

        // private async Task NotifyOrderNotFound() => 
        //     await _mediatorHandler.PublishNotification(new DomainNotification("Order", "Order not found"));


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