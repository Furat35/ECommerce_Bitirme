using ECommerce.Entity.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce.DataAccess.EntityTypeConfigurations
{
    public class OrderConfigurations : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("Orders");

            builder.HasKey(o => o.Id);

            builder.Property(o => o.TotalPrice)
                .IsRequired();

            builder.HasMany(o => o.OrderStatuses)
               .WithOne(u => u.Order)
               .HasForeignKey(o => o.OrderId)
               .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(o => o.ShippingPlace)
                .WithOne(o => o.Order)
                .HasForeignKey<ShippingPlace>(o => o.OrderId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(o => o.InvoiceInfo)
                .WithOne(o => o.Order)
                .HasForeignKey<InvoiceInfo>(o => o.OrderId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(o => o.OrderPaymentDetail)
                .WithOne(o => o.Order)
                .HasForeignKey<OrderPaymentDetail>(o => o.OrderId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
