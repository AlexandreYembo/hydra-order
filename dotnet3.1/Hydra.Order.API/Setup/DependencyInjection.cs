using FluentValidation.Results;
using Hydra.Catalog.Data.Repositories;
using Hydra.Core.Communication.Mediator;
using Hydra.Core.Messages.CommonMessages.Notifications;
using Hydra.Order.API.Application.Commands.Handlers;
using Hydra.Order.API.Application.Commands.OrderCommands;
using Hydra.Order.API.Application.Events.Handlers;
using Hydra.Order.API.Application.Events.OrderEvents;
using Hydra.Order.API.Application.Queries;
using Hydra.Order.Data;
using Hydra.Order.Domain.Repository;
using Hydra.WebAPI.Core.User;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Hydra.Order.API.Setup
{
    public static class DependencyInjection
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            //API
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IAspNetUser, AspNetUser>();

            //Application
            services.AddScoped<IMediatorHandler, MediatorHandler>();
            services.AddScoped<IOrderQueries, OrderQueries>();

            //Notifications
            services.AddScoped<INotificationHandler<DomainNotification>, DomainNotificationHandler>();

            //Data
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<OrderContext>();

            //Commands Order
            services.AddScoped<IRequestHandler<CreateOrderCommand, ValidationResult>, OrderCommandHandler>();

            //Events - Order
            services.AddScoped<INotificationHandler<OrderStartedEvent>, OrderEventHandler>();
        }
    }
}