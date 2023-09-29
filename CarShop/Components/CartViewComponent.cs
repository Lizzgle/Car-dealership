using Microsoft.AspNetCore.Mvc;

namespace CarShop.Views.Shared.Components
{
    public class Cart : ViewComponent
    {
        private decimal balance;
        private int countOfProducts;

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View();
        }
    }
}
