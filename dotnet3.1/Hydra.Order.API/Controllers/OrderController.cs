using System;
using System.Threading.Tasks;
using Hydra.Core.Communication.Mediator;
using Hydra.Core.Messages.CommonMessages.Notifications;
using Hydra.Order.API.Application.Commands.OrderCommands;
using Hydra.WebAPI.Core.Controllers;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Hydra.Order.API.Controllers
{
    public class OrderController : MainController
    {
        private readonly IMediatorHandler _mediatorHandler;


        public OrderController(INotificationHandler<DomainNotification> notifications,
                                IMediatorHandler mediatorHandler)
        {
            _mediatorHandler = mediatorHandler;
        }

        /// <summary>
        /// Endpoint that allows to create an order with new items or only update it.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="qty"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("order")]
        public async Task<IActionResult> CreateOrder()
        {
            //Implement method to check product exists, quantity in stock
            
            var command = new CreateOrderCommand(Guid.NewGuid(),20, 
                    new System.Collections.Generic.List<Application.DTO.OrderItemDTO>{
                        new Application.DTO.OrderItemDTO{
                            Amount = 15,
                            OrderId = Guid.NewGuid(),
                            Price = 145,
                            ProductId = Guid.NewGuid(),
                            ProductImage = "fdsfs.jpg",
                            ProductName = "MacBook pro",
                            Qty = 1
                        }
                    },
                    "AB-2020", true,  2, new Application.DTO.AddressDTO{
                    City = "Dublin",
                    Country = "Ireland",
                    Number= "36",
                    PostCode = "D2X45",
                    State = "Dublin",
                    Street = "Fitz Willian Street Lower"
            }, "1111222233334444", "Test Alex", "15/04/2025", "456");
            
            return CustomResponse(await _mediatorHandler.SendCommand(command));
        }
    }
}