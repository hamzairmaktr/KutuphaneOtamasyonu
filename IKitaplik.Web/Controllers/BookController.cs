using IKitaplik.Business.Abstract;
using IKitaplık.Entities.Concrete;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace IKitaplik.Web.Controllers
{
    public class BookController : Controller
    {
        IBookService _bookService;
        ICategoryService _categoryService;
        public BookController(IBookService bookService, ICategoryService categoryService)
        {
            _bookService = bookService;
            _categoryService = categoryService;
        }

        [HttpGet]
        public IActionResult Add()
        {
            var categoryList = _categoryService.GetAll();
            if (!categoryList.Success)
            {
                TempData["Message"] = categoryList.Message;
                TempData["MessageType"] = "danger";
            }
            ViewBag.Categories = new SelectList(categoryList.Data, "Id", "Name");
            return View();
        }

        [HttpPost]
        public IActionResult Add(Book book)
        {
            var res = _bookService.Add(book);
            if (res.Success)
            {
                TempData["Message"] = res.Message;
                TempData["MessageType"] = "success";
                return RedirectToAction("GetAll");
            }
            else
            {
                var categoryList = _categoryService.GetAll();
                if (!categoryList.Success)
                {
                    TempData["Message"] = categoryList.Message;
                    TempData["MessageType"] = "danger";
                }
                ViewBag.Categories = new SelectList(categoryList.Data, "Id", "Name");

                TempData["Message"] = res.Message;
                TempData["MessageType"] = "danger";
                return View(book);
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
                TempData["MessageType"] = "danger";
                return View();
            }
        }
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var res = _bookService.Delete(id);
            if (res.Success)
            {
                TempData["Message"] = res.Message;
                TempData["MessageType"] = "success";
                return RedirectToAction("GetAll");
            }
            else
            {
                TempData["Message"] = res.Message;
                TempData["MessageType"] = "danger";
                return RedirectToAction("GetAll");
            }
        }

        [HttpGet]
        public IActionResult Update(int id)
        {
            var res = _bookService.GetById(id);
            if (!res.Success)
            {
                TempData["Message"] = res.Message;
                TempData["MessageType"] = "danger"; 
                return RedirectToAction("GetAll");
            }

            var categoryList = _categoryService.GetAll();
            if (!categoryList.Success)
            {
                TempData["Message"] = categoryList.Message;
                TempData["MessageType"] = "danger";
            }
            ViewBag.Categories = new SelectList(categoryList.Data, "Id", "Name");
            return View(res.Data);
        }

        [HttpPost]
        public IActionResult Update(Book book)
        {
            var res = _bookService.Update(book);
            if (res.Success)
            {
                TempData["Message"] = res.Message;
                TempData["MessageType"] = "success";
                return RedirectToAction("GetAll");
            }
            else
            {
                var categoryList = _categoryService.GetAll();
                if (!categoryList.Success)
                {
                    TempData["Message"] = categoryList.Message;
                    TempData["MessageType"] = "danger";
                }
                ViewBag.Categories = new SelectList(categoryList.Data, "Id", "Name");

                TempData["Message"] = res.Message;
                TempData["MessageType"] = "danger";
                return View(book);
            }
        }
    }
}
