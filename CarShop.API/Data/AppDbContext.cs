using CarShop.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CarShop.API.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Car> Cars => Set<Car>();
        public DbSet<CarCategory> CarCategories => Set<CarCategory>();
        public AppDbContext(DbContextOptions<AppDbContext> options) : 
            base(options)
        {
            Database.EnsureCreated();
        }
    }
}
