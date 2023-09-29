using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CarShop.Controllers
{
    public class HomeController : Controller
    {
        private SelectList list = new(new List<ListDemo>()
        {
            new ListDemo() { Id = 1, Name = "Bob" },
            new ListDemo() { Id = 2, Name = "Bib" }
        },
        "Id", "Name");
        public IActionResult Index()
        {
            ViewData["Lab"] = "Лабораторная работа №2";
            return View(list);
        }

        public IActionResult Details() 
        {
            return PartialView();
        }
    }

    public class ListDemo
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
