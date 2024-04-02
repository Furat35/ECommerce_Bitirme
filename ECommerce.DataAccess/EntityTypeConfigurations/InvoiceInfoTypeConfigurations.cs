using ECommerce.Entity.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce.DataAccess.EntityTypeConfigurations
{
    public class InvoiceInfoTypeConfigurations : IEntityTypeConfiguration<InvoiceInfo>
    {
        public void Configure(EntityTypeBuilder<InvoiceInfo> builder)
        {
            builder.ToTable("InvoiceInfos");

            builder.HasKey(_ => _.Id);

            builder.Property(_ => _.ClientFullName)
                .IsRequired();
            builder.Property(_ => _.ClientPhone)
                .IsRequired();
            builder.Property(_ => _.TotalPrice)
                .IsRequired();

            builder.Ignore(_ => _.ModifiedDate);
            builder.Ignore(_ => _.DeletedDate);
            builder.Ignore(_ => _.IsValid);
        }
    }
}
