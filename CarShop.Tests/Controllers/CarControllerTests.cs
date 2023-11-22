using CarShop.Controllers;
using CarShop.Domain.Entities;
using CarShop.Domain.Models;
using CarShop.Services.CarCategoryService;
using CarShop.Services.CarService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Net;
using Xunit;

namespace CarShop.Tests.Controllers
{
    public class CarControllerTests
    {
        private readonly Mock<ICarService> _mockCarService;
        private readonly Mock<ICarCategoryService> _mockCategoryService;
        private readonly CarController _controller;
        public CarControllerTests() 
        {
            _mockCategoryService = new();
            _mockCarService = new();
            _controller = new(_mockCarService.Object,
                _mockCategoryService.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = new DefaultHttpContext()
                }
            };
        }


        [Fact]
        public void Index_InvalidCategoryList_ReturnsNotFound()
        {
            _mockCategoryService.Setup(service => service.GetCategoryListAsync().Result)
                .Returns(new Domain.Models.ResponseData<List<Domain.Entities.CarCategory>>() { Success = false });

            var result = _controller.Index(null, null).Result;

            Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal((int)HttpStatusCode.NotFound, ((NotFoundObjectResult)result).StatusCode);
        }

        [Fact]
        public void Index_InvalidCarList_ReturnsNotFound()
        {
            _mockCategoryService.Setup(service => service.GetCategoryListAsync().Result)
                .Returns(new ResponseData<List<Domain.Entities.CarCategory>>()
                {
                    Success = true,
                    Data = new List<CarCategory>()
                    {
                        new CarCategory () { Id = 1, Name = "Test 1", NormalizedName = "test1"},
                        new CarCategory() { Id = 2, Name = "Test 2", NormalizedName = "test2"},
                    }
                });

            _mockCarService.Setup(service => service.GetProductListAsync(null, 1).Result)
                .Returns(new Domain.Models.ResponseData<Domain.Models.ListModel<Domain.Entities.Car>>() { Success = false });

            var result = _controller.Index(1, null).Result;

            Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal((int)HttpStatusCode.NotFound, ((NotFoundObjectResult)result).StatusCode);
        }

        [Fact]
        public void Index_CorrectCarAndCategories_ViewDataContainsCategories()
        {
            _mockCategoryService.Setup(service => service.GetCategoryListAsync().Result)
                .Returns(new Domain.Models.ResponseData<List<Domain.Entities.CarCategory>>()
                {
                    Success = true,
                    Data = new List<Domain.Entities.CarCategory>()
                    {
                        new Domain.Entities.CarCategory() { Id = 1, Name = "Test 1", NormalizedName = "test1"},
                        new Domain.Entities.CarCategory() { Id = 2, Name = "Test 2", NormalizedName = "test2"},
                    }
                });

            _mockCarService.Setup(service => service.GetProductListAsync(null, 1).Result)
                .Returns(new Domain.Models.ResponseData<Domain.Models.ListModel<Domain.Entities.Car>>()
                {
                    Success = true,
                    Data = new ListModel<Car>()
                    {
                        CurrentPage = 1,
                        TotalPages = 1,
                        Items = new List<Car>()
                        {
                            new Car() { Id = 1, CategoryId = 1, Name = "Car 1", Price = 1},
                            new Car() { Id = 2, CategoryId = 1, Name = "Car 2", Price = 1},
                            new Car() { Id = 3, CategoryId = 1, Name = "Car 3", Price = 1},
                        }

                    }

                });



            var result = _controller.Index(1, null).Result;

            Assert.NotNull(result);

            var viewResult = Assert.IsType<ViewResult>(result);
            Type targetType = typeof(List<CarCategory>);
            Assert.Contains(viewResult.ViewData.Values, val => targetType.IsInstanceOfType(val));

        }

        [Fact]
        public void Index_CorrectCarsAndCategories_ViewDataContainsCurrectCategory()
        {
            _mockCategoryService.Setup(service => service.GetCategoryListAsync().Result)
                .Returns(new Domain.Models.ResponseData<List<Domain.Entities.CarCategory>>()
                {
                    Success = true,
                    Data = new List<Domain.Entities.CarCategory>()
                    {
                        new Domain.Entities.CarCategory() { Id = 1, Name = "Test 1", NormalizedName = "test1"},
                        new Domain.Entities.CarCategory() { Id = 2, Name = "Test 2", NormalizedName = "test2"},
                    }
                });

            _mockCarService.Setup(service => service.GetProductListAsync(null, 1).Result)
                .Returns(new Domain.Models.ResponseData<Domain.Models.ListModel<Domain.Entities.Car>>()
                {
                    Success = true,
                    Data = new ListModel<Car>()
                    {
                        CurrentPage = 1,
                        TotalPages = 1,
                        Items = new List<Car>()
                        {
                            new Car() { Id = 1, CategoryId = 1, Name = "Car 1", Price = 1},
                            new Car() { Id = 2, CategoryId = 1, Name = "Car 2", Price = 1},
                            new Car() { Id = 3, CategoryId = 1, Name = "Car 3", Price = 1},
                        }

                    }

                });



            var result = _controller.Index(1, null).Result;

            Assert.NotNull(result);

            var viewResult = Assert.IsType<ViewResult>(result);
            Type targetType = typeof(List<CarCategory>);
            Assert.Contains(viewResult.ViewData.Values,
                        val => (val is string str) && (str == "Все"));

        }

        [Fact]
        public void Index_CorrectCarsAndCategories_ModelIsFurnitureModelList()
        {
            _mockCategoryService.Setup(service => service.GetCategoryListAsync().Result)
                .Returns(new Domain.Models.ResponseData<List<Domain.Entities.CarCategory>>()
                {
                    Success = true,
                    Data = new List<Domain.Entities.CarCategory>()
                    {
                        new Domain.Entities.CarCategory() { Id = 1, Name = "Test 1", NormalizedName = "test1"},
                        new Domain.Entities.CarCategory() { Id = 2, Name = "Test 2", NormalizedName = "test2"},
                    }
                });

            _mockCarService.Setup(service => service.GetProductListAsync(null, 1).Result)
                .Returns(new Domain.Models.ResponseData<Domain.Models.ListModel<Domain.Entities.Car>>()
                {
                    Success = true,
                    Data = new ListModel<Car>()
                    {
                        CurrentPage = 1,
                        TotalPages = 1,
                        Items = new List<Car>()
                        {
                            new Car { Id = 1, CategoryId = 1, Name = "Car 1", Price = 1},
                            new Car { Id = 2, CategoryId = 1, Name = "Car 2", Price = 1},
                            new Car { Id = 3, CategoryId = 1, Name = "Car 3", Price = 1},
                        }

                    }

                });

            var result = _controller.Index(1, null).Result;

            Assert.NotNull(result);

            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsType<ListModel<Car>>(viewResult.Model);
        }
    }
}