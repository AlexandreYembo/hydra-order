using System;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation.Results;
using Hydra.Core.Communication.Mediator;
using Hydra.Core.Data;
using Hydra.Core.Extensions;
using Hydra.Core.Messages;
using Hydra.Order.Domain.Vouchers;
using Microsoft.EntityFrameworkCore;

namespace Hydra.Order.Data
{
    public class OrderContext : DbContext, IUnitOfWork
    {
        private readonly IMediatorHandler _mediatorHandler;

        public OrderContext(DbContextOptions<OrderContext> options, IMediatorHandler mediatorHandler) : base(options)
        {
            _mediatorHandler = mediatorHandler;
        }

        public DbSet<Domain.Orders.Order> Order {get;set;}
        public DbSet<Domain.Orders.OrderItem> OrderItem {get;set;}
        public DbSet<Voucher> Vourcher {get;set;}


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           foreach (var property in modelBuilder.Model.GetEntityTypes().SelectMany(
                e => e.GetProperties().Where(p => p.ClrType == typeof(string))))
                property.SetColumnType("varchar(100)"); // avoid do create any column NVarchar(MAX)

            modelBuilder.Ignore<ValidationResult>();
            //Use to Ignore event to persist on Database.
            modelBuilder.Ignore<Event>();

            //Does not need to add map for each element, new EF supports
            //It will find all entities and mapping defined on DbSet<TEntity> via reflection
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(OrderContext).Assembly);

            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(s => s.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.ClientSetNull;
            } 

            //It will generate an order number, starwith with 1000
            modelBuilder.HasSequence<int>("OrderSequence").StartsAt(1000).IncrementsBy(1);
            base.OnModelCreating(modelBuilder);
        }


        public async Task<bool> Commit()
        {
            //ChangeTracker -> EF: change mapper
            foreach (var entry in ChangeTracker.Entries().Where(entry => entry.Entity.GetType().GetProperty("CreatedDate") != null))
            {
                if(entry.State == EntityState.Added)
                    entry.Property("CreatedDate").CurrentValue = DateTime.Now;
                
                if(entry.State == EntityState.Modified)
                    entry.Property("CreatedDate").IsModified = false;   //Ignore to update any value set for the property = "CreatedDate"
            }

            var success = await base.SaveChangesAsync() > 0;
            if(success)
                await _mediatorHandler.PublishEvents(this); //Call the mediator extension to publish the events

            return success;
        }
    }
}