using Microsoft.AspNetCore.Mvc;

namespace BookStoreUI.Controllers
{
    public class OrderController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
