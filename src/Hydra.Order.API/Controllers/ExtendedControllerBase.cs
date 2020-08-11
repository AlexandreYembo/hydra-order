using System.Threading.Tasks;
using Hydra.Core.Communication.Mediator;
using Hydra.Core.Messages.CommonMessages.Notifications;
using Hydra.Order.API.Extensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Hydra.Order.API.Controllers
{
    public abstract class ExtendedControllerBase : ControllerBase
    {
        private readonly DomainNotificationHandler _notifications;      //TODO: Change to Interface in the future
        private readonly IMediatorHandler _mediatorHandler;

        protected ExtendedControllerBase(INotificationHandler<DomainNotification> notifications,
                                      IMediatorHandler mediatorHandler)
        {
            _notifications = (DomainNotificationHandler)notifications;      //TODO: Implement dependency inject using Interface
            _mediatorHandler = mediatorHandler;
        }

        /// <summary>
        /// If there is notification, there will have errors to notify
        /// </summary>
        /// <returns></returns>
        protected async Task<IActionResult> InvokeAsync(object result = null){
            if(_notifications.HasNotification())
                return BadRequest(new
                {
                    success = false,
                    errors = await new SummaryComponent(_notifications).InvokeAsync()
                });
            
            return Ok( new{
                success = true,
                data = result
            });

        }

        protected void NotifyNewError(string code, string message) => _mediatorHandler.PublishNotification(new DomainNotification(code, message));

    }
}