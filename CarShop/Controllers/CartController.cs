using CarShop.Domain.Models;
using CarShop.Services.CarService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CarShop.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly ICarService _carService;
        private readonly Cart _cart;
        public CartController(ICarService carService, Cart cart) 
        { 
            _carService = carService;
            _cart = cart;
        }

        public ActionResult Index()
        {
            return View(_cart.CartItems);
        }

        public ActionResult Delete(int id)
        {
            _cart.RemoveItems(id);

            return RedirectToAction("Index");
        }

        public ActionResult ClearAll()
        {
            _cart.ClearAll();

            return RedirectToAction("Index");
        }


        [Route("[controller]/add/{id:int}")]
        public async Task<ActionResult> Add(int id, string returnUrl)
        {
            var data = await _carService.GetProductByIdAsync(id);
            if (data.Success)
            {
                _cart.AddToCart(data.Data);
            }
            return Redirect(returnUrl);
        }
    }
}
