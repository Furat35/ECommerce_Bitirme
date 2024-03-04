using ECommerce.Entity.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce.DataAccess.EntityTypeConfigurations
{
    public class ProductConfigurations : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Products");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.ProductName)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(p => p.SubProductName)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(p => p.ProductDescription)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(p => p.Stock)
                .IsRequired();

            builder.Property(p => p.Price)
                .IsRequired();

            builder.HasMany(p => p.Feedbacks)
                .WithOne(f => f.Product)
                .HasForeignKey(f => f.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(p => p.ProductPhotos)
               .WithOne(f => f.Product)
               .HasForeignKey(f => f.ProductId)
               .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(p => p.SubCategory)
                .WithMany(sc => sc.Products)
                .HasForeignKey(p => p.SubCategoryId)
                .IsRequired();

            builder.HasOne(p => p.Brand)
                .WithMany(b => b.Products)
                .HasForeignKey(p => p.BrandId)
                .IsRequired();
        }
    }
}
