using ECommerce.Entity.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce.DataAccess.EntityTypeConfigurations
{
    public class AddressTypeConfigurations : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> builder)
        {
            builder.ToTable("Addresses");

            builder.HasKey(_ => _.Id);

            builder.Property(_ => _.Neighborhood)
                .IsRequired();
            builder.Property(_ => _.Street)
               .IsRequired();
            builder.Property(_ => _.Address1)
               .IsRequired();
            builder.Property(_ => _.Address2)
               .IsRequired();
            builder.Property(_ => _.DistrictId)
               .IsRequired();
        }
    }
}
