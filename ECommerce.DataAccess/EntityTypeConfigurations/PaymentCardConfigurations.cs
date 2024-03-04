using ECommerce.Entity.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce.DataAccess.EntityTypeConfigurations
{
    public class PaymentCardConfigurations : IEntityTypeConfiguration<PaymentCard>
    {
        public void Configure(EntityTypeBuilder<PaymentCard> builder)
        {
            builder.ToTable("PaymentCards");

            builder.HasOne(u => u.User)
                .WithOne(u => u.PaymentCard)
                .HasForeignKey<PaymentCard>(o => o.UserId)
                .OnDelete(DeleteBehavior.Restrict);

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
        }
    }
}
