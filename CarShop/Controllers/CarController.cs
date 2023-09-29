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
        public async Task<IActionResult> Index(string? category)
        {
            var productResponse = await _carService.GetProductListAsync(category);

            ResponseData<List<CarCategory>> categories = await _carCategoryService.GetCategoryListAsync();

            ViewData["currentCategory"] = categories.Data
                                            .FirstOrDefault(c => c.NormalizedName.Equals(category))?
                                            .Name ?? "Все";
            
            if (categories.Success)
            {
                ViewData["categories"] = categories.Data;
            }

            if (!productResponse.Success)
                return NotFound(productResponse.ErrorMessage);
            return View(productResponse.Data.Items);
        }
    }
}
