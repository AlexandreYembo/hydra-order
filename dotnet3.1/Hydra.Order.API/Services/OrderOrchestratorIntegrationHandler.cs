using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Hydra.Core.Integration.Messages.OrderMessages;
using Hydra.Core.MessageBus;
using Hydra.Order.API.Application.Queries;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Hydra.Order.API.Services
{
    //As alternative solution, there is a free library called hangfire.
    public class OrderOrchestratorIntegrationHandler : IHostedService, IDisposable
    {
        private readonly ILogger<OrderOrchestratorIntegrationHandler> _logger;
        private readonly IServiceProvider _serviceProvider;
        private Timer _timer;

        public OrderOrchestratorIntegrationHandler(ILogger<OrderOrchestratorIntegrationHandler> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Order service started.");
            //TimeSpan.Zero --> no delay
            //TimeSpan.FromSeconds(15) --> each 15 seconds will call the ProcessOrder
            _timer = new Timer(ProcessOrder, null, TimeSpan.Zero, TimeSpan.FromSeconds(15));
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Order service finished.");
            _timer.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer.Dispose();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="state"></param> State will controlled by the timer
        /// <returns></returns>
        private async void ProcessOrder(object state)
        {
            using var scope = _serviceProvider.CreateScope();
            var orderQueries = scope.ServiceProvider.GetRequiredService<IOrderQueries>();
            var order = await orderQueries.GetAuthorizedOrders();

            if(order == null) return;

            var bus = scope.ServiceProvider.GetRequiredService<IMessageBus>();

            //Send to the queue
            var authorizedOrder = new OrderAuthorizedIntegrationEvent(order.CustomerId, order.Id, order.Items.ToDictionary(oi => oi.ProductId, oi => oi.Qty));
            await bus.PublishAsync(authorizedOrder);
            
            _logger.LogInformation($"Order ID: {order.Id}  was sent to remove from the catalog stock");
        }
    } 
}