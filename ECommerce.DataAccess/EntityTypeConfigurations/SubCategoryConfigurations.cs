using ECommerce.Entity.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce.DataAccess.EntityTypeConfigurations
{
    public class SubCategoryConfigurations : IEntityTypeConfiguration<SubCategory>
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

            builder.HasOne(s => s.Category)
                .WithMany(s => s.SubCategories)
                .HasForeignKey(s => s.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(s => s.Products)
                .WithOne(s => s.SubCategory)
                .HasForeignKey(s => s.SubCategoryId);

        }
    }
}
