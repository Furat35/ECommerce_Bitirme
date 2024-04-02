using ECommerce.Entity.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce.DataAccess.EntityTypeConfigurations
{
    public class OrderItemProductTypeConfigurations : IEntityTypeConfiguration<OrderItemProduct>
    {
        public void Configure(EntityTypeBuilder<OrderItemProduct> builder)
        {
            builder.ToTable("OrderItemProducts");

            builder.HasKey(_ => _.Id);

            builder.Ignore(_ => _.CreatedDate);
            builder.Ignore(_ => _.ModifiedDate);
            builder.Ignore(_ => _.DeletedDate);
            builder.Ignore(_ => _.IsValid);
        }
    }
}
