using ECommerce.Core.Entities;
using ECommerce.Entity.Entities;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.DataAccess.Repositories.Contexts
{
    public class EfECommerceContext : DbContext
    {
        public EfECommerceContext(DbContextOptions<EfECommerceContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(EfECommerceContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            SetTimeStamps();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void SetTimeStamps()
        {
            var entities = ChangeTracker.Entries()
                .Where(x => x.Entity is BaseEntity && (x.State == EntityState.Added || x.State == EntityState.Modified || x.State == EntityState.Deleted));

            foreach (var entityEntry in entities)
            {
                var baseEntity = (BaseEntity)entityEntry.Entity;

                switch (entityEntry.State)
                {
                    case EntityState.Added:
                        baseEntity.CreatedDate = DateTime.UtcNow;
                        baseEntity.IsValid = true;
                        break;

                    case EntityState.Modified:
                        baseEntity.ModifiedDate = DateTime.UtcNow;
                        break;

                    case EntityState.Deleted:
                        baseEntity.DeletedDate = DateTime.UtcNow;
                        break;
                }
            }
        }

        public DbSet<Address> Addresses { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<District> Districts { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<InvoiceInfo> InvoiceInfos { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<OrderItemStatus> OrderItemStatuses { get; set; }
        public DbSet<OrderPaymentDetail> OrderPaymentDetails { get; set; }
        public DbSet<OrderStatus> OrderStatuses { get; set; }
        public DbSet<PaymentCard> PaymentCards { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductPhoto> ProductPhotos { get; set; }
        public DbSet<ShippingPlace> ShippingPlaces { get; set; }
        public DbSet<SubCategory> SubCategories { get; set; }
        public DbSet<OrderItemProduct> OrderItemProducts { get; set; }
    }
}
