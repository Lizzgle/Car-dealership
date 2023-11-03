using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using CarShop.Domain.Entities;
using CarShop.Services.CarService;
using CarShop.Services.CarCategoryService;
using CarShop.Domain.Models;

namespace CarShop.Areas.Admin.Pages
{
    public class CreateModel : PageModel
    {
        private readonly ICarService _carService;
        private readonly ICarCategoryService _categoryService;

        public SelectList Categories { get; set; }

        IFormFile? image; 

        [BindProperty]
        public IFormFile? Image { get
            {
                return image;
            } set {
                Console.WriteLine("Before ravno!!!");
                image = value;
                Console.WriteLine("Ater ravno!!!!");
            } }

        public CreateModel(ICarService carService, ICarCategoryService categoryService)
        {
            _carService = carService;
            _categoryService = categoryService;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            ResponseData<List<CarCategory>> requestCategories = await _categoryService.GetCategoryListAsync();

            if (requestCategories.Success)
            {
                Categories = new SelectList(requestCategories.Data, "Id", "Name");
            }

            return Page();
        }

        [BindProperty]
        public Car Car { get; set; } = default!;


        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid || _carService is null || Car == null)
            {
                return Page();
            }

            await _carService.CreateProductAsync(Car, Image);

            return RedirectToPage("./Index");
        }
    }
}
