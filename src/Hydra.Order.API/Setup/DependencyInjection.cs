
using Hydra.Catalog.Data.Repositories;
using Hydra.Order.Application.Commands;
using Hydra.Order.Domain.Repository;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Hydra.Order.API.Setup
{
    public static class DependencyInjection
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddScoped<IOrderRepository, OrderRepository>();

            //Notifications
            services.AddScoped<INotificationHandler<DomainNotification>, DomainNotificationHandler>();


            services.AddScoped<IRequestHandler<AddOrderItemCommand, bool>, OrderCommandHandler>();
        }
    }
}
