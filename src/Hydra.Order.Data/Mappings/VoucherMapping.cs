using Hydra.Order.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hydra.Order.Data.Mappings
{
    public class VoucherMapping : IEntityTypeConfiguration<Voucher>
    {
        public void Configure(EntityTypeBuilder<Voucher> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Code)
                   .IsRequired()
                   .HasColumnType("varchar(100)");

            // 1 : N => Voucher : Orders
            builder.HasMany(c => c.Order)
                   .WithOne(c => c.Voucher)
                   .HasForeignKey(c => c.VourcherId);

            builder.ToTable("Vouchers");
        }
    }
}