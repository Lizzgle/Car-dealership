using CarShop.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace CarShop.Views.Shared.Components
{
    public class CartViewComponent : ViewComponent
    {
        private readonly Cart _cart;

        public CartViewComponent(Cart cart)
        {
            _cart = cart;
        }

        public IViewComponentResult Invoke()
        {
            return View(_cart);
        }
    }
}
