using System.Threading;
using System.Threading.Tasks;
using FluentValidation.Results;
using Hydra.Core.Communication.Mediator;
using Hydra.Core.Integration.Messages;
using Hydra.Core.MessageBus;
using Hydra.Core.Messages;
using Hydra.Order.API.Application.Commands.CatalogCommands;
using Hydra.Order.API.Application.Events.CatalogEvents;
using MediatR;

namespace Hydra.Order.API.Application.Commands.Handlers
{
    public class CatalogCommandHandler : CommandHandler,
                                IRequestHandler<ProductValidationInStockCommand, ValidationResult>
    {
        private readonly IMessageBus _messageBus;
        private readonly IMediatorHandler _mediatorHandler;

        public CatalogCommandHandler(IMessageBus messageBus, IMediatorHandler mediatorHandler)
        {
            _messageBus = messageBus;
            _mediatorHandler = mediatorHandler;
        }
        
        public async Task<ValidationResult> Handle(ProductValidationInStockCommand message, CancellationToken cancellationToken)
        {
            var request = new CatalogIntegrationEvent(message.Products);
            var response = await _messageBus.RequestAsync<CatalogIntegrationEvent, ResponseMessage>(request);

            if(!response.ValidResult.IsValid){
                ValidationResult = response.ValidResult;
                await _mediatorHandler.PublishEvent(new ProductOutOfStockCheckedEvent(message.AggregateId));
                
                return ValidationResult;
            }

            await _mediatorHandler.PublishEvent(new ProductInStockCheckedEvent(message.AggregateId));
            
            return ValidationResult;
        }
    }
}