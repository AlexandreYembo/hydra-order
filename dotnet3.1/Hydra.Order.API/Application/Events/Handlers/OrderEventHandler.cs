using System.Threading;
using System.Threading.Tasks;
using Hydra.Core.Integration.Messages.OrderMessages;
using Hydra.Core.MessageBus;
using Hydra.Order.API.Application.Events.OrderEvents;
using MediatR;

namespace Hydra.Order.API.Application.Events.Handlers
{
    /// <summary>
    ///  All event that will be manipulated
    /// </summary>
    public class OrderEventHandler :
                        INotificationHandler<OrderStartedEvent>
                        // INotificationHandler<OrderDraftStartedEvent>,
                        // INotificationHandler<OrderStartedEvent>
    {
        private readonly IMessageBus _messageBus;

        public OrderEventHandler(IMessageBus messageBus)
        {
            _messageBus = messageBus;
        }

        /// <summary>
        /// Call the integration with the Queue
        /// </summary>
        /// <param name="notification"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task Handle(OrderStartedEvent message, CancellationToken cancellationToken) =>
            await _messageBus.PublishAsync(new OrderStartedIntegrationEvent(message.CustomerId, message.OrderId));


        // public async Task Handle(OrderStartedEvent message, CancellationToken cancellationToken) =>
        //     await _mediatorHandler.SendCommand(new ProductValidationInStockCommand(message.AggregateId, message.Products));

        // public Task Handle(OrderDraftStartedEvent notification, CancellationToken cancellationToken)
        // {
        //     throw new System.NotImplementedException();
        // }
    }
}