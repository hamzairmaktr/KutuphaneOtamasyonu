using IKitaplik.Business.Abstract;
using IKitaplik.Entities.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace IKitaplik.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin,User")]
    public class MovementController : ControllerBase
    {
        private readonly IMovementService _movementService;
        public MovementController(IMovementService movementService)
        {
            _movementService = movementService;
        }
        [HttpGet("getAll")]
        public async Task<IActionResult> GetAll([FromQuery] PageRequestDto page)
        {
            var res = await _movementService.GetAllAsync(page.Page, page.PageSize);
            if (!res.Success)
                return BadRequest(res);
            return Ok(res);
        }
        [HttpGet("getAllBookId")]
        public async Task<IActionResult> GetAllBookId(int id, [FromQuery] PageRequestDto page)
        {
            var res = await _movementService.GetAllFilteredBookIdAsync(id, page.Page, page.PageSize);
            if (!res.Success)
                return BadRequest(res);
            return Ok(res);
        }

        [HttpGet("getAllBookName")]
        public async Task<IActionResult> GetAllBookName(string bookName, [FromQuery] PageRequestDto pageRequestDto)
        {
            var res = await _movementService.GetAllFilteredBookNameAsync(bookName, pageRequestDto.Page, pageRequestDto.PageSize);
            if (!res.Success)
                return BadRequest(res);
            return Ok(res);
        }

        [HttpGet("getAllDepositId")]
        public async Task<IActionResult> GetAllDepositId(int id, [FromQuery] PageRequestDto pageRequestDto)
        {
            var res = await _movementService.GetAllFilteredDepositIdAsync(id, pageRequestDto.Page, pageRequestDto.PageSize);
            if (!res.Success)
                return BadRequest(res);
            return Ok(res);
        }

        [HttpGet("getAllStudentId")]
        public async Task<IActionResult> GetAllStudentId(int id, [FromQuery] PageRequestDto pageRequestDto)
        {
            var res = await _movementService.GetAllFilteredStudentIdAsync(id, pageRequestDto.Page, pageRequestDto.PageSize);
            if (!res.Success)
                return BadRequest(res);
            return Ok(res);
        }

        [HttpGet("getAllStudentName")]
        public async Task<IActionResult> GetAllStudentName(string studentName, [FromQuery] PageRequestDto pageRequestDto)
        {
            var res = await _movementService.GetAllFilteredStudentNameAsync(studentName, pageRequestDto.Page, pageRequestDto.PageSize);
            if (!res.Success)
                return BadRequest(res);
            return Ok(res);
        }

        [HttpGet("getAllDonationId")]
        public async Task<IActionResult> GetAllDonationId(int id, [FromQuery] PageRequestDto pageRequestDto)
        {
            var res = await _movementService.GetAllFilteredDonationIdAsync(id, pageRequestDto.Page, pageRequestDto.PageSize);
            if (!res.Success)
                return BadRequest(res);
            return Ok(res);
        }

        [HttpGet("getById")]
        public async Task<IActionResult> GetById(int id)
        {
            var res = await _movementService.GetByIdDtoAsync(id);
            if (!res.Success)
                return BadRequest(res);
            return Ok(res);
        }
    }
}
