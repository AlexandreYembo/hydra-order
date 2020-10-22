using System.Threading;
using System.Threading.Tasks;
using Hydra.Core.Communication.Mediator;
using Hydra.Order.API.Application.Commands.OrderCommands;
using Hydra.Order.API.Application.Commands.VoucherCommands;
using Hydra.Order.API.Application.Events.VoucherEvents;
using MediatR;

namespace Hydra.Order.API.Application.Events.Handlers
{
    public class VoucherEventHandler :
                        INotificationHandler<OrderWithoutVoucherVerifiedEvent>,
                        INotificationHandler<VoucherRefusedEvent>,
                        INotificationHandler<VoucherAppliedEvent>,
                        INotificationHandler<UpdateVoucherUsedFailedEvent>
    {

        private readonly IMediatorHandler _mediatorHandler;

        public VoucherEventHandler(IMediatorHandler mediatorHandler)
        {
            _mediatorHandler = mediatorHandler;
        }

         public async Task Handle(OrderWithoutVoucherVerifiedEvent message, CancellationToken cancellationToken) =>
            await _mediatorHandler.SendCommand(new StartOrderCommand(message.OrderId, message.CustomerId));
            
        public async Task Handle(VoucherRefusedEvent message, CancellationToken cancellationToken) =>
            await _mediatorHandler.SendCommand(new CancelOrderCommand(message.OrderId));

        public async Task Handle(VoucherAppliedEvent message, CancellationToken cancellationToken)
        {
            //Update the order as Start
            await _mediatorHandler.SendCommand(new StartOrderCommand(message.OrderId, message.CustomerId));

            //Update the voucher as used --> Voucher Microservice
            await _mediatorHandler.SendCommand(new UpdateVoucherUsedCommand(message.VoucherId, message.OrderId));
        }

        public async Task Handle(UpdateVoucherUsedFailedEvent message, CancellationToken cancellationToken) =>
            await _mediatorHandler.SendCommand(new CancelOrderCommand(message.OrderId));
    }
}