using ECommerce.Entity.Entities;
using Microsoft.Extensions.DependencyInjection;

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
                if (!context.Users.Any())
                {
                    await context.Users.AddRangeAsync(SeedUsersTable());
                    context.SaveChanges();
                }
                if (!context.Products.Any())
                {
                    await context.Products.AddRangeAsync(SeedProductsTable());
                    context.SaveChanges();
                }
                if (!context.Countries.Any())
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

        private List<User> SeedUsersTable()
            => new List<User>
            {
                new User{ Id = Guid.Parse("48df4af4-aa79-45f2-755c-08dc51134e88"), Name = "Company", Surname = "cc", Mail = "company@gmail.com", Phone = "5375655978",
                    Password = "war/WAa0EzB/LCXYZnjXzWp/nZ5DxsExyKhXhSM5HEI=", PasswordSalt = "bzl9bkpd2l3s2N/yWbMFww==", Role = Entity.Enums.Role.Company,
                    Company = new Company{ Mail = "rewr@gmail.com", CompanyName = "test comp", AboutCompany = "about comp", Phone= "52325523"} , CreatedDate = DateTime.Now, IsValid = true},
                  new User{ Id = Guid.Parse("321EAB18-CA1E-475A-B5E1-06871BB5D8D1"), Name = "Admin", Surname = "aa", Mail = "admin@gmail.com", Phone = "523525325", 
                      Password = "war/WAa0EzB/LCXYZnjXzWp/nZ5DxsExyKhXhSM5HEI=", PasswordSalt = "bzl9bkpd2l3s2N/yWbMFww==", Role = Entity.Enums.Role.Admin, CreatedDate = DateTime.Now, IsValid = true},
                    new User{ Id = Guid.Parse("621EAB18-CA1E-475A-B5E1-06871BB5D8D1"), Name = "User", Surname = "uu", Mail = "user@gmail.com", Phone = "523525325",
                      Password = "war/WAa0EzB/LCXYZnjXzWp/nZ5DxsExyKhXhSM5HEI=", PasswordSalt = "bzl9bkpd2l3s2N/yWbMFww==", Role = Entity.Enums.Role.User, CreatedDate = DateTime.Now, IsValid = true,
                    Cart = new Cart()}
            };

        private List<Product> SeedProductsTable()
            => new List<Product>
            {
                new Product{ ProductName = "İPhone 15", SubProductName = "256 gb siyah", CreatedBy = Guid.Parse("48DF4AF4-AA79-45F2-755C-08DC51134E88"),
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
