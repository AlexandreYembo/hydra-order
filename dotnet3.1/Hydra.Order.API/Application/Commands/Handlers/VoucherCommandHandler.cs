using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation.Results;
using Hydra.Core.Communication.Mediator;
using Hydra.Core.Integration.Messages;
using Hydra.Core.MessageBus;
using Hydra.Core.Messages;
using Hydra.Order.API.Application.Commands.VoucherCommands;
using Hydra.Order.API.Application.Events.VoucherEvents;
using MediatR;

namespace Hydra.Order.API.Application.Commands.Handlers
{
    public class VoucherCommandHandler : CommandHandler,
                                    IRequestHandler<UpdateVoucherUsedCommand, ValidationResult>
    {
        private readonly IMessageBus _messageBus;
        private readonly IMediatorHandler _mediatorHandler;
        public VoucherCommandHandler(IMessageBus messageBus, IMediatorHandler mediatorHandler)
        {
            _messageBus = messageBus;
            _mediatorHandler = mediatorHandler;
        }
        public async Task<ValidationResult> Handle(UpdateVoucherUsedCommand message, CancellationToken cancellationToken)
        {
            var voucherRequest = new VoucherIntegrationEvent(message.VoucherId);
            var response = await _messageBus.RequestAsync<VoucherIntegrationEvent, ResponseMessage>(voucherRequest);
            
            if(!response.ValidResult.IsValid)
            {
                await _mediatorHandler.PublishEvent(new UpdateVoucherUsedFailedEvent(message.VoucherId, message.OrderId));
                response.ValidResult.Errors.ToList().ForEach(e => AddError(e.ErrorMessage));
            }

            return ValidationResult;
        }
    }
}