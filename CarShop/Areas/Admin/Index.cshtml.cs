using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CarShop.API.Data;
using CarShop.Domain.Entities;
using CarShop.Services.CarService;
using CarShop.Domain.Models;

namespace CarShop.Areas.Admin
{
    public class IndexModel : PageModel
    {
        private readonly ICarService _carService;

        public IndexModel(ICarService carService)
        {
            _carService = carService;
        }

        public IList<Car> Car { get;set; } = default!;

        public async Task<IActionResult> OnGetAsync()
        {
            ResponseData<ListModel<Car>> requestCar = await _carService.GetProductListAsync(null);

            if (!requestCar.Success)
                return NotFound();

            Car = requestCar.Data.Items;
            return Page();
        }
    }
}
