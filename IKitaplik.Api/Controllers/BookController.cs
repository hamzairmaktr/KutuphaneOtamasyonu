using AutoMapper;
using IKitaplik.Business.Abstract;
using IKitaplik.Entities.Concrete;
using IKitaplik.Entities.DTOs.BookDTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IKitaplik.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        IBookService _bookService;
        IMapper _mapper;
        public BookController(IBookService bookService,IMapper mapper)
        {
            _bookService = bookService;
            _mapper = mapper;
        }

        [HttpGet("getall")]
        public ActionResult GetAll()
        {
            var res = _bookService.GetAll();
            if (res.Success)
                return Ok(res);
            return BadRequest(res);
        }
        [HttpPost("add")]
        public ActionResult Add([FromBody] BookAddDto bookAddDto)
        {
            var book = _mapper.Map<Book>(bookAddDto);
            var res = _bookService.Add(book);
            if (res.Success)
                return Ok(res);
            return BadRequest(res);
        }
        [HttpPut("bookAddPiece")]
        public ActionResult BookAddPiece([FromBody] Book book)
        {
            var res = _bookService.BookAddedPiece(book);
            if (res.Success)
                return Ok(res);
            return BadRequest(res);
        }
        [HttpPut("update")]
        public ActionResult Update([FromBody] Book book)
        {
            var res = _bookService.Update(book);
            if (res.Success)
                return Ok(res);
            return BadRequest(res);
        }
        [HttpDelete("delete")]
        public ActionResult Update([FromBody] int id)
        {
            var res = _bookService.Delete(id);
            if (res.Success)
                return Ok(res);
            return BadRequest(res);
        }
        [HttpGet("getallfilter")]
        public IActionResult GetAllFilter([FromQuery] BookFilterDto bookFilterDto)
        {
            var res = _bookService.GetAllFiltered(bookFilterDto);
            if (res.Success)
                return Ok(res);
            return BadRequest(res);
        }
        [HttpGet("getAllByName")]
        public IActionResult GetAllByName([FromQuery] string name)
        {
            var res = _bookService.GetAllByName(name);
            if (res.Success)
                return Ok(res);
            return BadRequest(res);
        }
        [HttpGet("getById")]
        public ActionResult GetById([FromQuery] int id)
        {
            var res = _bookService.GetById(id);
            if(res.Success)
                return Ok(res);
            return BadRequest(res);
        }

        [HttpGet("getByBarcode")]
        public ActionResult GetByBarcode([FromQuery] string barcode)
        {
            var res = _bookService.GetByBarcode(barcode);
            if (res.Success)
                return Ok(res);
            return BadRequest(res);
        }
    }
}
