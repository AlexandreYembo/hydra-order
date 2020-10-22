using System.Threading;
using System.Threading.Tasks;
using Hydra.Core.Communication.Mediator;
using Hydra.Order.API.Application.Commands.CatalogCommands;
using Hydra.Order.API.Application.Commands.VoucherCommands;
using Hydra.Order.API.Application.Events.OrderEvents;
using MediatR;

namespace Hydra.Order.API.Application.Events.Handlers
{
    /// <summary>
    ///  All event that will be manipulated
    /// </summary>
    public class OrderEventHandler :
                        INotificationHandler<OrderDraftStartedEvent>,
                        INotificationHandler<OrderStartedEvent>
    {
        private readonly IMediatorHandler _mediatorHandler;

        public OrderEventHandler(IMediatorHandler mediatorHandler)
        {
            _mediatorHandler = mediatorHandler;
        }

        public async Task Handle(OrderDraftStartedEvent message, CancellationToken cancellationToken) =>
             await _mediatorHandler.SendCommand(new ApplyVoucherOrderCommand(message.VoucherCode, message.CustomerId, message.OrderId, message.VoucherApplied));
 
        public async Task Handle(OrderStartedEvent message, CancellationToken cancellationToken) =>
            await _mediatorHandler.SendCommand(new ProductValidationInStockCommand(message.AggregateId, message.Products));

    }
}