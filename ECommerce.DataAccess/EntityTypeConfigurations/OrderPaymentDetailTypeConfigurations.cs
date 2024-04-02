using ECommerce.Entity.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce.DataAccess.EntityTypeConfigurations
{
    public class OrderPaymentDetailTypeConfigurations : IEntityTypeConfiguration<OrderPaymentDetail>
    {
        public void Configure(EntityTypeBuilder<OrderPaymentDetail> builder)
        {
            builder.ToTable("OrderPaymentDetails");

            builder.HasKey(_ => _.Id);

            builder.Property(_ => _.CardNumber)
                .IsRequired();
            builder.Property(_ => _.CVV)
               .IsRequired();
            builder.Property(_ => _.ExpireDate)
               .IsRequired();

            builder.Ignore(_ => _.ModifiedDate);
            builder.Ignore(_ => _.DeletedDate);
            builder.Ignore(_ => _.IsValid);
        }
    }
}
