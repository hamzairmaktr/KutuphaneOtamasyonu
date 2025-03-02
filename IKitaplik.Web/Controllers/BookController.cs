using IKitaplik.Business.Abstract;
using IKitaplik.Web.Models;
using IKitaplık.Entities.Concrete;
using IKitaplık.Entities.DTOs;
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
        public IActionResult GetAll(string? kitapAdi, int page = 1, int pageSize = 2)
        {
            var books = string.IsNullOrEmpty(kitapAdi)
                ? _bookService.GetAll()
                : _bookService.GetAllByName(kitapAdi);

            if (!books.Success)
            {
                TempData["Message"] = books.Message;
                TempData["MessageType"] = "danger";
                return View();
            }

            // Toplam kitap sayısı
            int totalBooks = books.Data.Count;

            // Sayfalama için kitapları filtreleyelim
            var paginatedBooks = books.Data.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            // Model olarak bir ViewModel gönderelim
            var model = new PaginatedList<BookGetDTO>
            {
                Items = paginatedBooks,
                PageIndex = page,
                TotalPages = (int)Math.Ceiling(totalBooks / (double)pageSize)
            };

            return View(model);
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
