using System;
using System.Threading;
using System.Threading.Tasks;
using Hydra.Core.DomainObjects;
using Hydra.Core.Integration.Messages.OrderMessages;
using Hydra.Core.MessageBus;
using Hydra.Order.Domain.Repository;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Hydra.Order.API.Services
{
    public class OrderIntegrationHandler : BackgroundService
    {
        private readonly IMessageBus _messageBus;
        private readonly IServiceProvider _serviceProvider;

        public OrderIntegrationHandler(IMessageBus messageBus, IServiceProvider serviceProvider)
        {
            _messageBus = messageBus;
            _serviceProvider = serviceProvider;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            throw new System.NotImplementedException();
        }

        private void SetSubscribers()
        {
            _messageBus.SubscribeAsync<OrderCanceledIntegrationEvent>("OrderCanceled", async request =>
                await CancelOrder(request));
            
            _messageBus.SubscribeAsync<OrderPaidIntegrationEvent>("OrderConfirmed", async request =>
                await ConfirmOrder(request));
        }

        private async Task CancelOrder(OrderCanceledIntegrationEvent message)
        {
            using var scope = _serviceProvider.CreateScope();
            var orderRepository = scope.ServiceProvider.GetRequiredService<IOrderRepository>();
            
            var order = await orderRepository.GetOrderById(message.OrderId);
            order.CancelOrder();

            orderRepository.UpdateOrder(order);

            if(!await orderRepository.UnitOfWork.Commit()) throw new DomainException($"An error occured to cancel the order {message.OrderId}");
        }

        private async Task ConfirmOrder(OrderPaidIntegrationEvent message)
        {
            using var scope = _serviceProvider.CreateScope();
            var orderRepository = scope.ServiceProvider.GetRequiredService<IOrderRepository>();
            
            var order = await orderRepository.GetOrderById(message.OrderId);
            order.ConfirmOrder();

            orderRepository.UpdateOrder(order);

            if(!await orderRepository.UnitOfWork.Commit()) throw new DomainException($"An error occured to confirm the order {message.OrderId}");
        }
    }
}