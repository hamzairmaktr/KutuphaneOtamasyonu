using Microsoft.AspNetCore.Mvc;

namespace IKitaplik.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
