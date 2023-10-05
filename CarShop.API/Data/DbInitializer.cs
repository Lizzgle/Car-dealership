using CarShop.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using static System.Reflection.Metadata.BlobBuilder;

namespace CarShop.API.Data
{
    public class DbInitializer
    {
        public static async Task SeedData(WebApplication app)
        {
            // Получение контекста БД
            using var scope = app.Services.CreateScope();
            var context =
            scope.ServiceProvider.GetRequiredService<AppDbContext>();

            // Выполнение миграций
            await context.Database.MigrateAsync();

            string pathImage = app.Configuration["ApiUrl"] ?? "";

            var categories = new List<CarCategory>()
            {
                new CarCategory {Name="Tesla",
                    NormalizedName="tesla"},
                new CarCategory {Name="BMW",
                    NormalizedName="bmw"},
                new CarCategory {Name="Porshe",
                    NormalizedName="porshe"},
                new CarCategory {Name="Lamborghini",
                    NormalizedName="lamborghini"},
            };
            await context.AddRangeAsync(categories);

            var cars = new List<Car>
                {
                new Car {Name="Tesla Model 3",
                Price =30000, Image="Images/tesla_model_3.png",
                Category = categories.Find(c=>c.NormalizedName.Equals("tesla"))},
            new Car {Name="Tesla Model Y",
                Price =39000, Image="Images/tesla_model_y.png",
                Category = categories.Find(c=>c.NormalizedName.Equals("tesla"))},
            new Car {Name="Tesla Model S",
                Price =71000, Image="Images/tesla_model_s.png",
                Category = categories.Find(c=>c.NormalizedName.Equals("tesla"))},
            new Car {Name="Tesla Model X",
                Price =69000, Image="Images/tesla_model_x.png",
                Category = categories.Find(c=>c.NormalizedName.Equals("tesla"))},

            new Car {Name="BMW i4",
                Price =59000, Image="Images/bmw_i4.png",
                Category = categories.Find(c=>c.NormalizedName.Equals("bmw"))},
            new Car {Name="BMW M4 Coupe",
                Price =136000, Image="Images/bmw_m4_coupe.png",
                Category = categories.Find(c=>c.NormalizedName.Equals("bmw"))},
            new Car {Name="BMW X6 M",
                Price =229000, Image="Images/bmw_x6_m.png",
                Category = categories.Find(c=>c.NormalizedName.Equals("bmw"))},
            new Car {Name="BMW Vision M NEXT",
                Price =444000, Image="Images/bmw_vision_m_next.png",
                Category = categories.Find(c=>c.NormalizedName.Equals("bmw"))},

            new Car {Name="Porshe 718 Cayman",
                Price =66000, Image="Images/porshe_718_cayman.png",
                Category = categories.Find(c=>c.NormalizedName.Equals("porshe"))},
            new Car {Name="Porshe 718 SPYDER",
                Price =110000, Image="Images/porshe_718_spyder.png",
                Category = categories.Find(c=>c.NormalizedName.Equals("porshe"))},
            new Car {Name="Porshe 911 GT3 RS",
                Price =260000, Image="Images/porshe_911_gt3_rs.png",
                Category = categories.Find(c=>c.NormalizedName.Equals("porshe"))},
            new Car {Name="Porshe Taycan",
                Price = 84000, Image="Images/porshe_taycan.png",
                Category = categories.Find(c=>c.NormalizedName.Equals("porshe"))},

            new Car {Name="Lamborghini HURACAN EVO SPYDER",
                Price =220000, Image="Images/lamborghini_huracan_evo_spyder.png",
                Category = categories.Find(c=>c.NormalizedName.Equals("lamborghini"))},
            new Car {Name="Lamborghini HURACAN TECNICA",
                Price =249000, Image="Images/lamborghini_huracan_tecnica.png",
                Category = categories.Find(c=>c.NormalizedName.Equals("lamborghini"))},
            new Car {Name="Lamborghini REVUELTO",
                Price =500000, Image="Images/lamborghini_revuelto.png",
                Category = categories.Find(c=>c.NormalizedName.Equals("lamborghini"))},
            new Car {Name="Lamborghini URUS S",
                Price =233000, Image="Images/lamborghini_urus_s.png",
                Category = categories.Find(c=>c.NormalizedName.Equals("lamborghini"))},
            };
            await context.AddRangeAsync(cars);

            await context.SaveChangesAsync();


        }
    }
}
