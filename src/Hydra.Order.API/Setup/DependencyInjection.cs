
using Hydra.Catalog.Data.Repositories;
using Hydra.Core.Communication.Mediator;
using Hydra.Core.Messages.CommonMessages.Notifications;
using Hydra.Order.Application.Commands;
using Hydra.Order.Application.Events;
using Hydra.Order.Application.Queries;
using Hydra.Order.Data;
using Hydra.Order.Domain.Repository;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Hydra.Order.API.Setup
{
    public static class DependencyInjection
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            //Mediator
            services.AddScoped<IMediatorHandler, MediatorHandler>();

            //Notifications
            services.AddScoped<INotificationHandler<DomainNotification>, DomainNotificationHandler>();
  
            //Repository
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<OrderContext>();

            //Commands
            services.AddScoped<IRequestHandler<AddOrderItemCommand, bool>, OrderCommandHandler>();
            services.AddScoped<IRequestHandler<UpdateOrderItemCommand, bool>, OrderCommandHandler>();
            services.AddScoped<IRequestHandler<RemoveOrderItemCommand, bool>, OrderCommandHandler>();
            services.AddScoped<IRequestHandler<ApplyVoucherOrderCommand, bool>, OrderCommandHandler>();
            

            //Queries
            services.AddScoped<IOrderQueries, OrderQueries>(); 

            //Events
            services.AddScoped<INotificationHandler<OrderDraftStartedEvent>, OrderEventHandler>();
            services.AddScoped<INotificationHandler<OrderUpdatedEvent>, OrderEventHandler>();
            services.AddScoped<INotificationHandler<OrderItemAddedEvent>, OrderEventHandler>();
        }
    }
}
