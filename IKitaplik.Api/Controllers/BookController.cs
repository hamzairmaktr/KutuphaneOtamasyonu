using AutoMapper;
using IKitaplik.Business.Abstract;
using IKitaplik.Entities.Concrete;
using IKitaplik.Entities.DTOs;
using IKitaplik.Entities.DTOs.BookDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

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
        public async Task<ActionResult> GetAll([FromQuery] PageRequestDto pageRequest)
        {
            var res = await _bookService.GetAllAsync(pageRequest.Page, pageRequest.PageSize);
            if (res.Success)
                return Ok(res);
            return BadRequest(res);
        }
        [HttpGet("getallisactive")]
        public async Task<ActionResult> GetAllActive([FromQuery] PageRequestDto pageRequestDto)
        {
            var res = await _bookService.GetAllActiveAsync(pageRequestDto.Page, pageRequestDto.PageSize);
            if (res.Success)
                return Ok(res);
            return BadRequest(res);
        }
        [HttpPost("add")]
        public async Task<ActionResult> Add([FromBody] BookAddDto bookAddDto)
        {
            var res = await _bookService.AddAsync(bookAddDto);
            if (res.Success)
                return Ok(res);
            return BadRequest(res);
        }
        [HttpPost("bookAddPiece")]
        public async Task<ActionResult> BookAddPiece([FromBody] BookAddPieceDto bookAddPieceDto)
        {
            var res = await _bookService.BookAddedPieceAsync(bookAddPieceDto);
            if (res.Success)
                return Ok(res);
            return BadRequest(res);
        }
        [HttpPost("update")]
        public async Task<ActionResult> Update([FromBody] BookUpdateDto bookUpdateDto)
        {
            var res = await _bookService.UpdateAsync(bookUpdateDto);
            if (res.Success)
                return Ok(res);
            return BadRequest(res);
        }
        [HttpPost("delete")]
        public async Task<ActionResult> Delete([FromBody] DeleteDto deleteDto)
        {
            var res = await _bookService.DeleteAsync(deleteDto.Id);
            if (res.Success)
                return Ok(res);
            return BadRequest(res);
        }
        [HttpGet("getallfilter")]
        public async Task<IActionResult> GetAllFilter([FromQuery] BookFilterDto bookFilterDto, [FromQuery] PageRequestDto page)
        {
            var res = await _bookService.GetAllFilteredAsync(page.Page, page.PageSize, bookFilterDto);
            if (res.Success)
                return Ok(res);
            return BadRequest(res);
        }
        [HttpGet("getAllByName")]
        public async Task<IActionResult> GetAllByName([FromQuery] string name)
        {
            var res = await _bookService.GetAllByNameAsync(name);
            if (res.Success)
                return Ok(res);
            return BadRequest(res);
        }
        [HttpGet("getById")]
        public async Task<ActionResult> GetById([FromQuery] int id)
        {
            var res = await _bookService.GetByIdAsync(id);
            if (res.Success)
                return Ok(res);
            return BadRequest(res);
        }

        [HttpGet("getByBarcode")]
        public async Task<ActionResult> GetByBarcode([FromQuery] string barcode)
        {
            var res = await _bookService.GetByBarcodeAsync(barcode);
            if (res.Success)
                return Ok(res);
            return BadRequest(res);
        }

        [HttpPost("bookAddedPiece")]
        public async Task<IActionResult> BookAddedPiece(BookAddPieceDto bookAddPieceDto)
        {
            var res = await _bookService.BookAddedPieceAsync(bookAddPieceDto);
            if (res.Success)
                return Ok(res);
            return BadRequest(res);
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string query)
        {
            var result = await _bookService.SearchForAutocompleteAsync(query);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpGet("getDtoById")]
        public async Task<IActionResult> GetDtoById([FromQuery] int id)
        {
            var res = await _bookService.GetDtoByIdAsync(id);
            if (res.Success)
                return Ok(res);
            return BadRequest(res);
        }
    }
}
