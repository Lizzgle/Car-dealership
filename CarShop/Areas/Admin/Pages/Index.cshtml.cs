using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CarShop.Domain.Entities;
using CarShop.Services.CarService;
using CarShop.Domain.Models;

namespace CarShop.Areas.Admin.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ICarService _carService;

        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }

        public IndexModel(ICarService carService)
        {
            _carService = carService;
        }

        public IList<Car> Car { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? pageno)
        {
            ResponseData<ListModel<Car>> requestCar = await _carService.GetProductListAsync(null, pageno ?? -1);

            if (!requestCar.Success)
                return NotFound();

            Car = requestCar.Data.Items;
            TotalPages = requestCar.Data.TotalPages;
            CurrentPage = requestCar.Data.CurrentPage;
            return Page();
        }
    }
}
