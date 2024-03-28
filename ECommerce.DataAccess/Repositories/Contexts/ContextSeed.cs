using ECommerce.Core.Helpers.PasswordServices;
using ECommerce.Entity.Entities;
using Microsoft.Extensions.DependencyInjection;
using System.Text;

namespace ECommerce.DataAccess.Repositories.Contexts
{
    public class ContextSeed
    {
        private readonly IServiceProvider _serviceProvider;

        public ContextSeed(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task SeedDatabaseAsync()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<EfECommerceContext>();
                context.Database.EnsureCreated();
                if (!context.Products.Any())
                {
                    await context.Products.AddRangeAsync(SeedProductsTable());
                    context.SaveChanges();
                }
                if(!context.Countries.Any())
                {
                    await context.Countries.AddRangeAsync(SeedCountriesTable());
                    context.SaveChanges();
                }
                if (!context.Cities.Any())
                {
                    await context.Cities.AddRangeAsync(SeedCitiesTable());
                    context.SaveChanges();
                }
                if (!context.Districts.Any())
                {
                    await context.Districts.AddRangeAsync(SeedDistrictsTable());
                    context.SaveChanges();
                }
            }
        }

        private List<Product> SeedProductsTable()
            => new List<Product>
            {
                new Product{ ProductName = "İPhone 15", SubProductName = "256 gb siyah", UserId = Guid.Parse("121EAB18-CA1E-475A-B5E1-06871BB5D8D1"),
                    ProductDescription = "Özellikleri vs.", Stock = 50, Price = 222,
                    SubCategory = new SubCategory{Name = "Telefon", IsValid = true, CreatedDate = DateTime.Now,
                    Category = new Category{ Name = "Elektronik", IsValid = true, CreatedDate = DateTime.Now}},
                    Brand = new Brand{ Name = "Apple",CreatedDate = DateTime.Now, IsValid = true},
                    IsValid = true, CreatedDate = DateTime.Now  }
            };

        private List<Country> SeedCountriesTable()
            => new List<Country>
            {
               new Country
               {
                    Id = Guid.Parse("4E45810C-C5BD-46BD-B634-B3981D8BE55F"),
                    CountryName = "Türkiye",
                    IsValid = true,
                    CreatedDate = DateTime.Now
               },
               new Country
               {
                    Id = Guid.Parse("7E45810C-C5BD-46BD-B634-B3981D8BE55A"),
                    CountryName = "Almanya",
                    IsValid = true,
                    CreatedDate = DateTime.Now
               }
            };

        private List<City> SeedCitiesTable()
         => new List<City>
         {
               new City
               {
                    Id = Guid.Parse("7745810C-C5BD-46BD-B634-B3981D8BE55A"),
                    CountryId = Guid.Parse("4E45810C-C5BD-46BD-B634-B3981D8BE55F"),
                    CityName = "İstabul",
                    IsValid = true,
                    CreatedDate = DateTime.Now
               },
               new City
               {
                    Id = Guid.Parse("5545810C-C5BD-46BD-B634-B3981D8BE55F"),
                    CountryId = Guid.Parse("4E45810C-C5BD-46BD-B634-B3981D8BE55F"),
                    CityName = "İzmir",
                    IsValid = true,
                    CreatedDate = DateTime.Now
               },
               new City
               {
                    Id = Guid.Parse("8845810C-C5BD-46BD-B634-B3981D8BE55A"),
                    CountryId = Guid.Parse("7E45810C-C5BD-46BD-B634-B3981D8BE55A"),
                    CityName = "Hanov",
                    IsValid = true,
                    CreatedDate = DateTime.Now
               }
         };

        private List<District> SeedDistrictsTable()
            => new List<District>
            {
                new District
                {
                   DistrictName = "Karşıyaka",
                   CityId = Guid.Parse("5545810C-C5BD-46BD-B634-B3981D8BE55F"),
                   CreatedDate = DateTime.Now,
                   IsValid = true
                },
                new District
                {
                   DistrictName = "Kadıköy",
                   CityId = Guid.Parse("7745810C-C5BD-46BD-B634-B3981D8BE55A"),
                   CreatedDate = DateTime.Now,
                   IsValid = true
                },
                new District
                {
                   DistrictName = "Hanov",
                   CityId = Guid.Parse("8845810C-C5BD-46BD-B634-B3981D8BE55A"),
                   CreatedDate = DateTime.Now,
                   IsValid = true
                },
                new District
                {
                   DistrictName = "Üsküdar",
                   CityId = Guid.Parse("7745810C-C5BD-46BD-B634-B3981D8BE55A"),
                   CreatedDate = DateTime.Now,
                   IsValid = true
                },
                new District
                {
                   DistrictName = "Çiğli",
                   CityId = Guid.Parse("5545810C-C5BD-46BD-B634-B3981D8BE55F"),
                   CreatedDate = DateTime.Now,
                   IsValid = true
                }
            };
    }
}
