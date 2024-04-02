using ECommerce.Entity.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce.DataAccess.EntityTypeConfigurations
{
    public class OrderItemTypeConfigurations : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.ToTable("OrderItems");

            builder.HasKey(_ => _.Id);

            builder.Ignore(_ => _.CreatedDate);
            builder.Ignore(_ => _.ModifiedDate);
            builder.Ignore(_ => _.DeletedDate);
            builder.Ignore(_ => _.IsValid);
        }
    }
}
