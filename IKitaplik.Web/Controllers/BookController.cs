using IKitaplik.Business.Abstract;
using IKitaplık.Entities.Concrete;
using Microsoft.AspNetCore.Mvc;

namespace IKitaplik.Web.Controllers
{
    public class BookController : Controller
    {
        IBookService _bookService;
        public BookController(IBookService bookService)
        {
            _bookService = bookService;
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add(Book book)
        {
            var res = _bookService.Add(book);
            if (res.Success)
            {
                TempData["Message"] = res.Message;
                TempData["MessageType"] = "success"; // Başarılı mesaj
                return RedirectToAction("GetAll");
            }
            else
            {
                TempData["Message"] = res.Message;
                TempData["MessageType"] = "danger"; // Başarılı mesaj
                return View();
            }
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var res = _bookService.GetAll();
            if (res.Success)
            {
                return View(res.Data);
            }
            else
            {
                TempData["Message"] = res.Message;
                TempData["MessageType"] = "danger"; // Başarılı mesaj
                return View();
            }
        }
    }
}
