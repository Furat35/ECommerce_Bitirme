using ECommerce.Entity.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce.DataAccess.EntityTypeConfigurations
{
    public class UserTypeConfigurations : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");

            builder.HasKey(u => u.Id);

            builder.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(p => p.Surname)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(p => p.Mail)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(p => p.Phone)
               .HasMaxLength(15);

            builder.Property(p => p.Password)
               .IsRequired();

            builder.Property(p => p.PasswordSalt)
               .IsRequired();

            builder.Property(p => p.Role)
                .IsRequired();

            builder.HasOne(u => u.Cart)
                .WithOne(u => u.User)
                .HasForeignKey<Cart>(o => o.Id)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(u => u.Company)
                .WithOne(u => u.User)
                .HasForeignKey<Company>(o => o.Id)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(u => u.Address)
                .WithOne(u => u.User)
                .HasForeignKey<Address>(o => o.Id)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(u => u.PaymentCard)
                .WithOne(u => u.User)
                .HasForeignKey<PaymentCard>(o => o.Id)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
