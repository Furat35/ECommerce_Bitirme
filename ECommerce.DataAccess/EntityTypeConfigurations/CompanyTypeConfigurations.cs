using ECommerce.Entity.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce.DataAccess.EntityTypeConfigurations
{
    public class CompanyTypeConfigurations : IEntityTypeConfiguration<Company>
    {
        public void Configure(EntityTypeBuilder<Company> builder)
        {
            builder.ToTable("Companies");

            builder.HasKey(_ => _.Id);
            builder.Property(_ => _.CompanyName)
                .IsRequired();
            builder.Property(_ => _.Phone)
               .IsRequired();
            builder.Property(_ => _.Mail)
               .IsRequired();
            builder.Property(_ => _.AboutCompany)
               .IsRequired();
            builder.Property(_ => _.UserId)
               .IsRequired();
        }
    }
}
