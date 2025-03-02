using IKitaplýk.Entities.DTOs;
using IKitaplik.Business.Abstract;
using IKitaplik.Web.Models;
using Microsoft.AspNetCore.Mvc;
using IKitaplýk.Entities.Concrete;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace IKitaplik.Web.Controllers
{
    public class StudentController : Controller
    {
        IStudentService _studentService;
        public StudentController(IStudentService studentService)
        {
            _studentService = studentService;
        }
        [HttpGet]
        public IActionResult GetAll(string? studentName, int page = 1, int pageSize = 10)
        {
            var students = string.IsNullOrEmpty(studentName)
                ? _studentService.GetAll()
                : _studentService.GetAllByName(studentName);

            if (!students.Success)
            {
                TempData["Message"] = students.Message;
                TempData["MessageType"] = "danger";
                return View();
            }

            // Toplam kitap sayýsý
            int totalBooks = students.Data.Count;

            // Sayfalama için kitaplarý filtreleyelim
            var paginatedBooks = students.Data.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            // Model olarak bir ViewModel gönderelim
            var model = new PaginatedList<Student>
            {
                Items = paginatedBooks,
                PageIndex = page,
                TotalPages = (int)Math.Ceiling(totalBooks / (double)pageSize)
            };

            return View(model);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Add(Student student)
        {
            var res = _studentService.Add(student);
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
                return View(student);
            }
        }
    }
}
