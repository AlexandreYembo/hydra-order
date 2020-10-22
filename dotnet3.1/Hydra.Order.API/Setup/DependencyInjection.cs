using FluentValidation.Results;
using Hydra.Catalog.Data.Repositories;
using Hydra.Core.Communication.Mediator;
using Hydra.Core.Messages.CommonMessages.Notifications;
using Hydra.Order.API.Application.Commands.CatalogCommands;
using Hydra.Order.API.Application.Commands.Handlers;
using Hydra.Order.API.Application.Commands.OrderCommands;
using Hydra.Order.API.Application.Commands.VoucherCommands;
using Hydra.Order.API.Application.Events.CatalogEvents;
using Hydra.Order.API.Application.Events.Handlers;
using Hydra.Order.API.Application.Events.OrderEvents;
using Hydra.Order.API.Application.Events.VoucherEvents;
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

            //Notifications
            services.AddScoped<INotificationHandler<DomainNotification>, DomainNotificationHandler>();

            //Data
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<OrderContext>();

            //Commands Order
            services.AddScoped<IRequestHandler<CreateOrderCommand, ValidationResult>, OrderCommandHandler>();
            services.AddScoped<IRequestHandler<ApplyVoucherOrderCommand, ValidationResult>, OrderCommandHandler>();
            services.AddScoped<IRequestHandler<StartOrderCommand, ValidationResult>, OrderCommandHandler>();

            //Commands - Voucher
            services.AddScoped<IRequestHandler<UpdateVoucherUsedCommand, ValidationResult>, VoucherCommandHandler>();

            //Commmands - Catalog
            services.AddScoped<IRequestHandler<ProductValidationInStockCommand, ValidationResult>, CatalogCommandHandler>();

            //Events - Order
            services.AddScoped<INotificationHandler<OrderDraftStartedEvent>, OrderEventHandler>();
            services.AddScoped<INotificationHandler<OrderStartedEvent>, OrderEventHandler>();

            //Events - Voucher
            services.AddScoped<INotificationHandler<OrderWithoutVoucherVerifiedEvent>, VoucherEventHandler>();
            services.AddScoped<INotificationHandler<VoucherRefusedEvent>, VoucherEventHandler>();
            services.AddScoped<INotificationHandler<VoucherAppliedEvent>, VoucherEventHandler>();
            services.AddScoped<INotificationHandler<UpdateVoucherUsedFailedEvent>, VoucherEventHandler>();

            //Events - catalog
            services.AddScoped<INotificationHandler<ProductOutOfStockCheckedEvent>, CatalogEventHandler>();
            services.AddScoped<INotificationHandler<ProductInStockCheckedEvent>, CatalogEventHandler>();
        }
    }
}