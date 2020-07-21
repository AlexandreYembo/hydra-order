using System.Linq;
using System.Threading.Tasks;
using Hydra.Core.DomainObjects;
using Hydra.Order.Data;
using MediatR;

namespace Hydra.Data.Extensions
{
    public static class MediatorExtension
        {
            /// <summary>
            /// Extension that check in the entity for all notification and then it will send them.
            /// </summary>
            /// <param name="mediator"></param>
            /// <param name="ctx"></param>
            /// <returns></returns>
            public static async Task OrderEventPublish(this IMediator mediator, OrderContext ctx)
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
                        await mediator.Publish(domainEvents);
                    });

                await Task.WhenAll(tasks);
            }
    }
}