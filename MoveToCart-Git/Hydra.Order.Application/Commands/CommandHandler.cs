using Hydra.Core.DomainObjects;
using Hydra.Core.Messages;
using MediatR;

namespace Hydra.Order.Application.Commands
{
    public abstract class CommandHandler
    {
        private readonly IMediator _mediator;
        
        protected CommandHandler(IMediator mediator)
        {
            _mediator = mediator;
        }
        public virtual bool IsCommandValid(Command message)
        {
            if(message.IsValid()) return true;

            foreach (var error in message.ValidationResult.Errors)
            {
                _mediator.Publish(new DomainNotification(message.MessageType, error.ErrorMessage));
            }

            return false;
        }
    }
}