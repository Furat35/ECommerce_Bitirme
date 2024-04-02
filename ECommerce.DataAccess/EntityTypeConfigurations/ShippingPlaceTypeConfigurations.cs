using ECommerce.Entity.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce.DataAccess.EntityTypeConfigurations
{
    public class ShippingPlaceTypeConfigurations : IEntityTypeConfiguration<ShippingPlace>
    {
        public void Configure(EntityTypeBuilder<ShippingPlace> builder)
        {
            builder.ToTable("ShippingPlaces");

            builder.HasKey(_ => _.Id);

            builder.Property(_ => _.DistrictId)
                .IsRequired();
            builder.Property(_ => _.Neighborhood)
               .IsRequired();
            builder.Property(_ => _.Street)
               .IsRequired();
            builder.Property(_ => _.Address1)
               .IsRequired();
            builder.Property(_ => _.Address2)
               .IsRequired();

            builder.Ignore(_ => _.CreatedDate);
            builder.Ignore(_ => _.ModifiedDate);
            builder.Ignore(_ => _.DeletedDate);
            builder.Ignore(_ => _.IsValid);
        }
    }
}
