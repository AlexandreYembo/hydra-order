using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace Hydra.Order.Application.Events
{
    /// <summary>
    ///  All event that will be manipulated
    /// </summary>
    public class OrderEventHandler :
        INotificationHandler<OrderDraftStartedEvent>,
        INotificationHandler<OrderItemAddedEvent>,
        INotificationHandler<OrderUpdatedEvent>
    {
        public Task Handle(OrderDraftStartedEvent notification, CancellationToken cancellationToken)
        {
            //TODO Implement. Could be a queue, persist on Readonly Database
            return Task.CompletedTask;
        }

        public Task Handle(OrderItemAddedEvent notification, CancellationToken cancellationToken)
        {
           //TODO Implement. Could be a queue, persist on Readonly Database
            return Task.CompletedTask;
        }

        public Task Handle(OrderUpdatedEvent notification, CancellationToken cancellationToken)
        {
             //TODO Implement. Could be a queue, persist on Readonly Database
            return Task.CompletedTask;
        }
    }
}