using ECommerce.Entity.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce.DataAccess.EntityTypeConfigurations
{
    public class OrderItemStatusTypeConfigurations : IEntityTypeConfiguration<OrderItemStatus>
    {
        public void Configure(EntityTypeBuilder<OrderItemStatus> builder)
        {
            builder.Ignore(_ => _.ModifiedDate);
            builder.Ignore(_ => _.DeletedDate);
        }
    }
}
