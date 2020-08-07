using System.Linq;
using System.Threading.Tasks;
using Hydra.Core.Communication.Mediator;
using Hydra.Core.DomainObjects;
using Hydra.Order.Data;

namespace Hydra.Data.Extensions
{
    public static class MediatorExtension
        {
            /// <summary>
            /// Extension that check in the entity for all notification and then it will send them.
            /// Extension to Publish List of Events
            /// </summary>
            /// <param name="mediator"></param>
            /// <param name="ctx"></param>
            /// <returns></returns>
            public static async Task PublishEvents(this IMediatorHandler mediator, OrderContext ctx)
            {
                var domainEntities = ctx.ChangeTracker
                    .Entries<Entity>()
                    .Where(x => x.Entity.Notifications != null && x.Entity.Notifications.Any());

                var domainEvents = domainEntities
                    .SelectMany(x => x.Entity.Notifications)
                    .ToList();

                domainEntities.ToList()
                    .ForEach(entity => entity.Entity.ClearEvents());

                var tasks = domainEvents
                    .Select(async (domainEvent) => {
                        await mediator.PublishEvent(domainEvent);
                    });

                await Task.WhenAll(tasks);
            }
    }
}