using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hydra.Core.Bus;
using Hydra.Order.Application.Commands;
using Hydra.Order.Application.Interfaces.Services;
using Hydra.Order.Application.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;


namespace Hydra.Order.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderAppServices _orderAppService;
        private readonly IMediatorHandler _mediatorHandler;

        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<OrderController> _logger;

        public OrderController(ILogger<OrderController> logger, 
                                IOrderAppServices orderAppService, 
                                IMediatorHandler mediatorHandler)
        {
            _logger = logger;
            _orderAppService = orderAppService;
            _mediatorHandler = mediatorHandler;
        }

        [HttpPost]
        public async Task<ActionResult> CreateOrder(OrderDto order)
        {
        //     var command = new AddOrderItemCommand(Guid.NewGuid(), product)
        //    _orderAppService.AddOrder(order)

        //     return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> AddOrderItem(Guid id, int qty)
        {
            //Implement method to check product exists, quantity in stock
            
            var command = new AddOrderItemCommand(Guid.NewGuid(), Guid.NewGuid(), "Product Test",  qty, 12.2m);
            await _mediatorHandler.SendCommand(command);

            

            return Ok(result);
        }

        [HttpPut]
        public async Task<ActionResult> UpdateOrder()
        {
            var rng = new Random();
            var result = Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();

            return Ok(result);
        }

        [HttpGet]
        public async Task<ActionResult> GetOrder()
        {
            var rng = new Random();
            var result = Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();

            return Ok(result);
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteOrder()
        {
            var rng = new Random();
            var result = Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();

            return Ok(result);
        }
    }
}