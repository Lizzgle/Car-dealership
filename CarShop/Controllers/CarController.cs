using CarShop.Domain.Entities;
using CarShop.Domain.Models;
using CarShop.Services.CarCategoryService;
using CarShop.Services.CarService;
using Microsoft.AspNetCore.Mvc;

namespace CarShop.Controllers
{
    public class CarController : Controller
    {
        private readonly ICarService _carService;
        private readonly ICarCategoryService _carCategoryService;
        public CarController(ICarService carService, ICarCategoryService categoryService)
        {
            _carService = carService;
            _carCategoryService = categoryService;
        }
        public async Task<IActionResult> Index()
        {
            var productResponse = await _carService.GetProductListAsync(null);
            if (!productResponse.Success)
                return NotFound(productResponse.ErrorMessage);
            return View(productResponse.Data.Items);
        }
    }
}
