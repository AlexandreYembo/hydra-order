using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hydra.Order.Data.Mappings
{
    public class OrderMapping : IEntityTypeConfiguration<Domain.Models.Order>
    {
        public void Configure(EntityTypeBuilder<Domain.Models.Order> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Code)
                   .HasDefaultValueSql("NEXT VALUE FOR OrderSequence");

                    // 1 : N => Order : OrderItems
            builder.HasMany(c => c.OrderItems)
                   .WithOne(c => c.Order)
                   .HasForeignKey(c => c.OrderId);

            builder.ToTable("Orders");
        }
    }
}