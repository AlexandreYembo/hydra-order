using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hydra.Order.Data.Mappings
{
    public class OrderMapping : IEntityTypeConfiguration<Domain.Orders.Order>
    {
        public void Configure(EntityTypeBuilder<Domain.Orders.Order> builder)
        {
            builder.HasKey(c => c.Id);

            builder.OwnsOne(o => o.Address, e => //Convert the object value in columns for the Order table
            {
                e.Property(oe => oe.Street)
                    .HasColumnName("StreetName");
                
                e.Property(oe => oe.Number)
                    .HasColumnName("Number");

                e.Property(oe => oe.City)
                    .HasColumnName("City");

                e.Property(oe => oe.State)
                    .HasColumnName("State");
                
                e.Property(oe => oe.Country)
                    .HasColumnName("Country");

                e.Property(oe => oe.PostCode)
                    .HasColumnName("PostCode");
            });

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