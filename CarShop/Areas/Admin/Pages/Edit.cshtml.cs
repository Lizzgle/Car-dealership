using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using CarShop.Domain.Entities;
using CarShop.Services.CarService;
using CarShop.Domain.Models;
using CarShop.Services.CarCategoryService;

namespace CarShop.Areas.Admin.Pages
{
    public class EditModel : PageModel
    {
        private readonly ICarService _carService;
        private readonly ICarCategoryService _categoryService;

        public SelectList Categories { get; set; }

        public EditModel(ICarService carService, ICarCategoryService carCategoryService)
        {
            _carService = carService;
            _categoryService = carCategoryService;
        }

        [BindProperty]
        public Car Car { get; set; } = default!;

        [BindProperty]
        public IFormFile? Image { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _carService is null)
            {
                return NotFound();
            }

            var car = await _carService.GetProductByIdAsync(id ?? -1);
            if (!car.Success)
            {
                return NotFound();
            }
            Car = car.Data;

            ResponseData<List<CarCategory>> requestCategories = await _categoryService.GetCategoryListAsync();
            Categories = new SelectList(requestCategories.Data, "Id", "Name");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _carService.UpdateProductAsync(Car.Id, Car, Image);

            return RedirectToPage("./Index");
        }
    }
}
