using System.Linq;
using System.Threading.Tasks;
using Hydra.Core.Data;
using Hydra.Core.DomainObjects;
using Hydra.Data.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Hydra.Order.Data
{
    public class OrderContext : DbContext, IUnitOfWork
    {
        private readonly IMediator _mediator;

        public OrderContext(DbContextOptions<OrderContext> options, IMediator mediator) : base(options)
        {
            _mediator = mediator;
        }
        public async Task<bool> Commit()
        {
            var success = await base.SaveChangesAsync() > 0;

            if(success) await _mediator.OrderEventPublish(this);

            return success;
        }
    }
}