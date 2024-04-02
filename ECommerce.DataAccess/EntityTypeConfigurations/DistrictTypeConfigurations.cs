using ECommerce.Entity.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce.DataAccess.EntityTypeConfigurations
{
    public class DistrictTypeConfigurations : IEntityTypeConfiguration<District>
    {
        public void Configure(EntityTypeBuilder<District> builder)
        {
            builder.ToTable("Districts");

            builder.HasKey(_ => _.Id);

            builder.Property(p => p.DistrictName)
                 .IsRequired()
                 .HasMaxLength(100);

            builder.Ignore(_ => _.CreatedDate);
            builder.Ignore(_ => _.ModifiedDate);
            builder.Ignore(_ => _.DeletedDate);
        }
    }
}
