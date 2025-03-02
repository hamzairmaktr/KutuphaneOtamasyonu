using Microsoft.AspNetCore.Mvc;

namespace IKitaplik.Web.Controllers
{
    public class StudentController : Controller
    {
        // GET: StudentController
        public ActionResult GetAll()
        {
            return View();
        }

    }
}
