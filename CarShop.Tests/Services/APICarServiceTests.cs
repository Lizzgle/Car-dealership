using CarShop.API.Data;
using CarShop.API.Services;
using CarShop.Domain.Entities;
using CarShop.Domain.Models;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CarShop.Tests.Services
{
    public class SqliteInMemoryAPIFurnitureServiceTests : IDisposable
    {
        private readonly DbConnection _connection;
        private readonly DbContextOptions<AppDbContext> _options;

        public SqliteInMemoryAPIFurnitureServiceTests()
        {
            _connection = new SqliteConnection("Filename=:memory:");
            _connection.Open();

            _options = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlite(_connection)
                .Options;

            using var context = new AppDbContext(_options);
            context.Database.EnsureCreated();

            context.CarCategories.AddRange(new CarCategory[]
            {
                new CarCategory {Name="Audi",
                NormalizedName="audi"},
                new CarCategory {Name="Jaguar",
                NormalizedName="jaguar"},
            });

            context.Cars.AddRange(new Car[] {
                new Car()
                {
                    Name = "e-tron GT quattro",
                    Image = null,
                    Price = 200000,
                    CategoryId = 1,
                },
                new Car()
                {
                    Name = "RS 6 Avant",
                    Image = null,
                    Price = 110000,
                    CategoryId = 1,
                },
                new Car()
                {
                    Name = "A5 Sportback",
                    Image = null,
                    Price = 100000,
                    CategoryId = 1,
                },
                
                new Car()
                {
                    Name = " I-PACE",
                    Image = null,
                    Price = 92000,
                    CategoryId = 2,
                },
                new Car()
                {
                    Name = "F‑TYPE",
                    Image = null,
                    Price = 247000,
                    CategoryId = 2,
                },
                new Car()
                {
                    Name = "TCS RACING",
                    Image = null,
                    Price = 116000,
                    CategoryId = 2,
                },});

            context.SaveChanges();
        }

        private AppDbContext CreateContext() => new(_options);

        public void Dispose() => _connection.Dispose();

        [Fact]
        public void GetProductList_ReturnsFirstPageOf3Items()
        {
            using var context = CreateContext();
            var service = new CarService(context, null, null);
            var result = service.GetProductListAsync(null).Result;
            Assert.IsType<ResponseData<ListModel<Car>>>(result);
            Assert.True(result.Success);
            Assert.Equal(1, result.Data.CurrentPage);
            Assert.Equal(3, result.Data.Items.Count);
            Assert.Equal(2, result.Data.TotalPages);
            Assert.Equal(context.Cars.First(), result.Data.Items[0]);
        }

        [Fact]
        public void GetProductList_ReturnsCorrectPage()
        {
            using var context = CreateContext();
            var service = new CarService(context, null, null);
            var result = service.GetProductListAsync(null, 2).Result;

            Assert.IsType<ResponseData<ListModel<Car>>>(result);
            Assert.True(result.Success);
            Assert.Equal(2, result.Data.CurrentPage);
        }

        [Fact]
        public void GetProductList_ReturnsCorrectCategoryCars()
        {
            using var context = CreateContext();
            var service = new CarService(context, null, null);
            var result = service.GetProductListAsync("audi").Result;

            Assert.IsType<ResponseData<ListModel<Car>>>(result);
            Assert.True(result.Success);
            Assert.Equal(1, result.Data.TotalPages);
            Assert.True(result.Data.Items.All(f => f.Category.NormalizedName.Equals("audi")));
        }

        [Fact]
        public void GetProductList_NotEditMaxPageSize()
        {
            using var context = CreateContext();
            var service = new CarService(context, null, null);
            var result = service.GetProductListAsync(null, 1, 25).Result;
            Assert.IsType<ResponseData<ListModel<Car>>>(result);
            Assert.True(result.Success);
            Assert.Equal(context.Cars.Count(), result.Data.Items.Count);
        }

        [Fact]
        public void GetProductList_ReturnsFalseSuccess()
        {
            using var context = CreateContext();
            var service = new CarService(context, null, null);
            var result = service.GetProductListAsync(null, 10).Result;
            Assert.IsType<ResponseData<ListModel<Car>>>(result);
            Assert.False(result.Success);
        }
    }
}
