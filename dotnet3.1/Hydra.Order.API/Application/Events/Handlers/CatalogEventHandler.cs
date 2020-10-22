using System.Threading;
using System.Threading.Tasks;
using Hydra.Core.Communication.Mediator;
using Hydra.Order.API.Application.Commands.OrderCommands;
using Hydra.Order.API.Application.Events.CatalogEvents;
using MediatR;

namespace Hydra.Order.API.Application.Events.Handlers
{
    public class CatalogEventHandler :
                            INotificationHandler<ProductOutOfStockCheckedEvent>,
                            INotificationHandler<ProductInStockCheckedEvent>
    {
        private readonly IMediatorHandler _mediatorHandler;

        public CatalogEventHandler(IMediatorHandler mediatorHandler)
        {
            _mediatorHandler = mediatorHandler;
        }

        public async Task Handle(ProductOutOfStockCheckedEvent message, CancellationToken cancellationToken) =>
            await _mediatorHandler.SendCommand(new CancelOrderCommand(message.AggregateId));

        public async Task Handle(ProductInStockCheckedEvent message, CancellationToken cancellationToken) => 
            await _mediatorHandler.SendCommand(new ProcessingOrderCommand(message.OrderId));
    }
}