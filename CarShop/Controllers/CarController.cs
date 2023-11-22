using CarShop.Domain.Entities;
using CarShop.Domain.Models;
using CarShop.Extensions;
using CarShop.Services.CarCategoryService;
using CarShop.Services.CarService;
using Microsoft.AspNetCore.Mvc;

namespace CarShop.Controllers
{
    public class CarController : Controller
    {
        private readonly ICarService _carService;
        private readonly ICarCategoryService _carCategoryService;
        public CarController(ICarService carService, 
            ICarCategoryService categoryService)
        {
            _carService = carService;
            _carCategoryService = categoryService;
        }
        [Route("Catalog")]
        [Route("Catalog/{category}")]
        public async Task<IActionResult> Index(int? pageno, string? category)
        {
            var productResponse = await _carService.GetProductListAsync(category, pageno ?? 1);

            ResponseData<List<CarCategory>> categories = await _carCategoryService.GetCategoryListAsync();

            if (!categories.Success)
                return NotFound(categories.ErrorMessage);

            ViewData["currentCategory"] = categories.Data
                                            .FirstOrDefault(c => c.NormalizedName.Equals(category))?
                                            .Name ?? "Все";
            ViewData["category"] = category;
            ViewData["categories"] = categories.Data;

            if (!productResponse.Success)
                return NotFound(productResponse.ErrorMessage);

            if (Request.IsAjaxRequest())
            {
                return PartialView("_CarsPartial", productResponse.Data);
            }
            return View("Index",productResponse.Data);
        }
    }
}
