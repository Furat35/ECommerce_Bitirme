using ECommerce.Core.Helpers.PasswordServices;
using ECommerce.Entity.Entities;
using Microsoft.Extensions.DependencyInjection;
using System.Text;

namespace ECommerce.DataAccess.Repositories.Contexts
{
    public class ContextSeed
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IPasswordGenerationService _passwordGenerationService;

        public ContextSeed(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _passwordGenerationService = serviceProvider.GetService<IPasswordGenerationService>();
        }

        public async Task SeedDatabaseAsync()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<EfECommerceContext>();
                context.Database.EnsureCreated();
                if (!context.Products.Any() || !context.Feedbacks.Any() || !context.SubCategories.Any() || !context.Brands.Any() || !context.Categories.Any())
                {
                    await context.Products.AddRangeAsync(SeedProductsTable());
                    await context.Feedbacks.AddRangeAsync(SeedFeedbackTable());
                    await context.SubCategories.AddRangeAsync(SeedSubCategoryTable());
                    await context.Brands.AddRangeAsync(SeedBrandTable());
                    await context.Categories.AddRangeAsync(SeedCategoryTable());
                    await context.Users.AddRangeAsync(SeedUser());
                    await context.SaveChangesAsync();
                }
            }
        }

        private List<Product> SeedProductsTable()
            => new List<Product>
            {
                new Product{ Id = Guid.Parse("666EAB18-CA1E-475A-B5E1-06871BB5D8D6"), ProductName = "İPhone 15", SubProductName = "256 gb siyah", UserId = Guid.Parse("121EAB18-CA1E-475A-B5E1-06871BB5D8D1"),
                    ProductDescription = "Özellikleri vs.", Stock = 50, Price = 222, SubCategoryId = Guid.Parse("777EAB18-CA1E-475A-B5E1-06871BB5D8D6"), BrandId = Guid.Parse("333EAB18-CA1E-475A-B5E1-06871BB5D8D6") }
            };

        private List<Feedback> SeedFeedbackTable()
            => new List<Feedback>
            {
                new Feedback{ Id =  Guid.Parse("952EAB18-CA1E-475A-B5E1-06871BB5D8D6"), UserFeedback = "çok beğendim", ProductId = Guid.Parse("666EAB18-CA1E-475A-B5E1-06871BB5D8D6")}
            };

        private List<SubCategory> SeedSubCategoryTable()
            => new List<SubCategory>
            {
                new SubCategory{ Id = Guid.Parse("777EAB18-CA1E-475A-B5E1-06871BB5D8D6"), Name = "Telefon", CategoryId = Guid.Parse("111EAB18-CA1E-475A-B5E1-06871BB5D8D6")}
            };

        private List<Brand> SeedBrandTable()
            => new List<Brand>
            {
                new Brand{ Id = Guid.Parse("333EAB18-CA1E-475A-B5E1-06871BB5D8D6"), Name = "İPhone" }
            };

        private List<Category> SeedCategoryTable()
            => new List<Category>
            {
                new Category{ Id = Guid.Parse("111EAB18-CA1E-475A-B5E1-06871BB5D8D6"), Name = "Elektronik" }
            };

        private List<User> SeedUser()
        {
            var salt = _passwordGenerationService.GenerateSalt();
            var combinedBytes = _passwordGenerationService.Combine(Encoding.UTF8.GetBytes("12345"), salt);
            var hashedBytes = _passwordGenerationService.HashBytes(combinedBytes);
            string storedSalt = Convert.ToBase64String(salt);
            string storedHashedPassword = Convert.ToBase64String(hashedBytes);
            var user = new User { Id = Guid.Parse("121EAB18-CA1E-475A-B5E1-06871BB5D8D1"), Name = "User", Surname = "UU", Mail = "user@gmail.com", Role = Entity.Enums.Role.User, Password = storedHashedPassword, PasswordSalt = storedSalt, Phone = "5326253", Cart = new Cart { Id = Guid.Parse("331EAB18-CA1E-475A-B5E1-06871BB5D8D0"), UserId = Guid.Parse("121EAB18-CA1E-475A-B5E1-06871BB5D8D1") } };
            var company = new User { Id = Guid.Parse("83DD7A85-7D11-4474-7E3C-08DC3BE17468"), Name = "Company", Surname = "CC", Mail = "company@gmail.com", Role = Entity.Enums.Role.Company, Password = storedHashedPassword, PasswordSalt = storedSalt, Phone = "436264632", Company = new Company { Id = Guid.Parse("881EAB18-CA1E-475A-B5E1-06871BB5D8D1"), CompanyName = "Apple", Phone = "53232626", Mail = "company@gmail.com", AboutCompany = "iyi bir şirket", UserId = Guid.Parse("83DD7A85-7D11-4474-7E3C-08DC3BE17468") } };
            var admin = new User { Id = Guid.Parse("671EAB18-CA1E-475A-B5E1-06871BB5D8D1"), Name = "Admin", Surname = "AA", Mail = "admin@gmail.com", Role = Entity.Enums.Role.Admin, Password = storedHashedPassword, PasswordSalt = storedSalt, Phone = "64326236" };
            return new List<User> { user, company, admin };
        }

    }
}
