using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CarShop.Domain.Entities;
using CarShop.Services.CarService;

namespace CarShop.Areas.Admin.Pages
{
    public class DetailsModel : PageModel
    {
        private readonly ICarService _carService;

        public DetailsModel(ICarService carService)
        {
            _carService = carService;
        }

        public Car Car { get; set; } = default!;

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
            else
            {
                Car = car.Data;
            }
            return Page();
        }
    }
}
