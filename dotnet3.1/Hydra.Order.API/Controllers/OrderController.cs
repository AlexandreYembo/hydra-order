using System.Threading.Tasks;
using Hydra.Core.Communication.Mediator;
using Hydra.Order.API.Application.Commands.OrderCommands;
using Hydra.Order.API.Application.Queries;
using Hydra.WebAPI.Core.Controllers;
using Hydra.WebAPI.Core.Identity;
using Hydra.WebAPI.Core.User;
using Microsoft.AspNetCore.Mvc;

namespace Hydra.Order.API.Controllers
{
    public class OrderController : MainController
    {
        private readonly IMediatorHandler _mediatorHandler;
        private readonly IAspNetUser _user;
        private readonly IOrderQueries _orderQueries;


        public OrderController(IMediatorHandler mediatorHandler,
                                IAspNetUser user,
                                IOrderQueries orderQueries)
        {
            _mediatorHandler = mediatorHandler;
            _user = user;
            _orderQueries = orderQueries;
        }

        /// <summary>
        /// Endpoint that allows to create an order with new items or only update it.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="qty"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("order/create")]
        [ClaimsAuthorize("order", "write")]
        public async Task<IActionResult> CreateOrder(CreateOrderCommand order)
        {
            //Implement method to check product exists, quantity in stock
            order.CustomerId = _user.GetUserId();
            return CustomResponse(await _mediatorHandler.SendCommand(order));
        }

        [HttpGet("order/latest")]
        public async Task<IActionResult> LatestOrder()
        {
            var order = await _orderQueries.GetLatestOrder(_user.GetUserId());
            return order == null ? NotFound() : CustomResponse(order);
        }

        [HttpGet("order/listByCustomer")]
        public async Task<IActionResult> OrdersByCustomer()
        {
            var order = await _orderQueries.GetOrdersByCustomerId(_user.GetUserId());
            return order == null ? NotFound() : CustomResponse(order);
        }
    }
}