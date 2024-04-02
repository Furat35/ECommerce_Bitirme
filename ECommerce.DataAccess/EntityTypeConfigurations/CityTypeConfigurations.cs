using ECommerce.Entity.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce.DataAccess.EntityTypeConfigurations
{
    public class CityTypeConfigurations : IEntityTypeConfiguration<City>
    {
        public void Configure(EntityTypeBuilder<City> builder)
        {
            builder.ToTable("Cities");

            builder.HasKey(_ => _.Id);

            builder.Property(p => p.CityName)
                 .IsRequired()
                 .HasMaxLength(100);

            builder.Ignore(_ => _.CreatedDate);
            builder.Ignore(_ => _.ModifiedDate);
            builder.Ignore(_ => _.DeletedDate);
        }
    }
}
