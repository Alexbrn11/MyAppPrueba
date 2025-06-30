using Microsoft.AspNetCore.Mvc;

namespace MyApp.Presentation.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

