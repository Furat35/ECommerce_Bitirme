using ECommerce.Entity.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce.DataAccess.EntityTypeConfigurations
{
    public class ProductTypeConfigurations : IEntityTypeConfiguration<Product>
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
        }
    }
}
