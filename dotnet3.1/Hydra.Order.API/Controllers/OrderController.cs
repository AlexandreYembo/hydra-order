using System;
using System.Threading.Tasks;
using Hydra.Core.Communication.Mediator;
using Hydra.Core.Messages.CommonMessages.Notifications;
using Hydra.Order.API.Models;
using Hydra.Order.Application.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Hydra.WebAPI.Core.Controllers;

namespace Hydra.Order.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : MainController
    {
        private readonly IMediatorHandler _mediatorHandler;

        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<OrderController> _logger;

        public OrderController(INotificationHandler<DomainNotification> notifications,
                                ILogger<OrderController> logger, 
                                IMediatorHandler mediatorHandler) : base(notifications, mediatorHandler)
        {
            _logger = logger;
            _mediatorHandler = mediatorHandler;
        }

        // [HttpPost]
        // public async Task<ActionResult> CreateOrder(OrderDto order)
        // {
        // //     var command = new AddOrderItemCommand(Guid.NewGuid(), product)
        // //    _orderAppService.AddOrder(order)

        // //     return Ok(result);
        // }

        /// <summary>
        /// Endpoint that allows to create an order with new items or only update it.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="qty"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> AddOrderItem([FromBody] OrderItemDto dto)
        {
            //Implement method to check product exists, quantity in stock
            
            var command = new AddOrderItemCommand(Guid.NewGuid(), Guid.NewGuid(), "Product Test",  dto.Qty, 12.2m);
            await _mediatorHandler.SendCommand(command);

            return await InvokeAsync();
        }

        // [HttpPut]x
        // public async Task<ActionResult> UpdateOrder()
        // {
        //     var rng = new Random();
        //     var result = Enumerable.Range(1, 5).Select(index => new WeatherForecast
        //     {
        //         Date = DateTime.Now.AddDays(index),
        //         TemperatureC = rng.Next(-20, 55),
        //         Summary = Summaries[rng.Next(Summaries.Length)]
        //     })
        //     .ToArray();

        //     return Ok(result);
        // }

        // [HttpGet]
        // public async Task<ActionResult> GetOrder()
        // {
        //     var rng = new Random();
        //     var result = Enumerable.Range(1, 5).Select(index => new WeatherForecast
        //     {
        //         Date = DateTime.Now.AddDays(index),
        //         TemperatureC = rng.Next(-20, 55),
        //         Summary = Summaries[rng.Next(Summaries.Length)]
        //     })
        //     .ToArray();

        //     return Ok(result);
        // }

        // [HttpDelete]
        // public async Task<ActionResult> DeleteOrder()
        // {
        //     var rng = new Random();
        //     var result = Enumerable.Range(1, 5).Select(index => new WeatherForecast
        //     {
        //         Date = DateTime.Now.AddDays(index),
        //         TemperatureC = rng.Next(-20, 55),
        //         Summary = Summaries[rng.Next(Summaries.Length)]
        //     })
        //     .ToArray();

        //     return Ok(result);
        // }
    }
}