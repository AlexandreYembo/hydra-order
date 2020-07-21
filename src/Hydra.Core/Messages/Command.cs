using System;
using FluentValidation.Results;
using MediatR;

namespace Hydra.Core.Messages
{
    public abstract class Command : Message, IRequest<bool>
    {
        public DateTime TimeStamp { get; private set; }
        public ValidationResult ValidationResult { get; set; }

        protected Command()
        {
            TimeStamp = DateTime.Now;
        }

        public abstract bool IsValid();
    }
}