using AutoMapper;
using IKitaplik.Business.Abstract;
using IKitaplik.Entities.Concrete;
using IKitaplik.Entities.DTOs;
using IKitaplik.Entities.DTOs.BookDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IKitaplik.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin,User")]
    public class BookController : ControllerBase
    {
        IBookService _bookService;
        public BookController(IBookService bookService)
        {
            _bookService = bookService;
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
            var res = _bookService.Add(bookAddDto);
            if (res.Success)
                return Ok(res);
            return BadRequest(res);
        }
        [HttpPost("bookAddPiece")]
        public ActionResult BookAddPiece([FromBody] BookAddPieceDto bookAddPieceDto)
        {
            var res = _bookService.BookAddedPiece(bookAddPieceDto);
            if (res.Success)
                return Ok(res);
            return BadRequest(res);
        }
        [HttpPost("update")]
        public ActionResult Update([FromBody] BookUpdateDto bookUpdateDto)
        {
            var res = _bookService.Update(bookUpdateDto);
            if (res.Success)
                return Ok(res);
            return BadRequest(res);
        }
        [HttpPost("delete")]
        public ActionResult Delete([FromBody] DeleteDto deleteDto)
        {
            var res = _bookService.Delete(deleteDto.Id);
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

        [HttpPost("bookAddedPiece")]
        public IActionResult BookAddedPiece(BookAddPieceDto bookAddPieceDto)
        {
            var res = _bookService.BookAddedPiece(bookAddPieceDto);
            if(res.Success)
                return Ok(res);
            return BadRequest(res);
        }
    }
}
