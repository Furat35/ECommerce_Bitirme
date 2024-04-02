using ECommerce.Entity.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce.DataAccess.EntityTypeConfigurations
{
    public class SubCategoryTypeConfigurations : IEntityTypeConfiguration<SubCategory>
    {
        public void Configure(EntityTypeBuilder<SubCategory> builder)
        {
            builder.ToTable("SubCategories");

            builder.HasKey(s => s.Id);

            builder.Property(s => s.Name)
                          .IsRequired()
                          .HasMaxLength(50);

            builder.Property(s => s.CategoryId)
                .IsRequired();
        }
    }
}
