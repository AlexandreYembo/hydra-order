using Hydra.Core.Communication.Mediator;
using Hydra.Core.DomainObjects;
using Hydra.Core.Messages;
using Hydra.Core.Messages.CommonMessages.Notifications;
using MediatR;

namespace Hydra.Order.Application.Commands
{
    public abstract class CommandHandler
    {
        private readonly IMediatorHandler _mediatorHandler;
        
        protected CommandHandler(IMediatorHandler mediatorHandler)
        {
            _mediatorHandler = mediatorHandler;
        }
        public virtual bool IsCommandValid(Command message)
        {
            if(message.IsValid()) return true;

            foreach (var error in message.ValidationResult.Errors)
            {
                //MessageType -> name of the class
                _mediatorHandler.PublishNotification(new DomainNotification(message.MessageType, error.ErrorMessage));
            }

            return false;
        }
    }
}