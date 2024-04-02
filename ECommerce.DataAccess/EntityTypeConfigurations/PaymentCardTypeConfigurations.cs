using ECommerce.Entity.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce.DataAccess.EntityTypeConfigurations
{
    public class PaymentCardTypeConfigurations : IEntityTypeConfiguration<PaymentCard>
    {
        public void Configure(EntityTypeBuilder<PaymentCard> builder)
        {
            builder.ToTable("PaymentCards");

            builder.HasKey(_ => _.Id);

            builder.Property(_ => _.NameSurname)
                .IsRequired()
                .HasMaxLength(70);
            builder.Property(_ => _.CardNumber)
                .IsRequired()
                .HasMaxLength(16)
                .IsFixedLength();
            builder.Property(_ => _.CVV)
               .IsRequired()
               .HasMaxLength(3)
               .IsFixedLength();
            builder.Property(_ => _.ExpireDate)
              .IsRequired();
            builder.Property(_ => _.UserId)
            .IsRequired();
        }
    }
}
