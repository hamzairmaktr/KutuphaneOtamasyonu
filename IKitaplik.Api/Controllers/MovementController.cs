using IKitaplik.Business.Abstract;
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
        public async Task<IActionResult> GetAll()
        {
            var res = await _movementService.GetAllAsync();
            if (!res.Success)
                return BadRequest(res);
            return Ok(res);
        }
        [HttpGet("getAllBookId")]
        public async Task<IActionResult> GetAllBookId(int id)
        {
            var res = await _movementService.GetAllFilteredBookIdAsync(id);
            if (!res.Success)
                return BadRequest(res);
            return Ok(res);
        }

        [HttpGet("getAllBookName")]
        public async Task<IActionResult> GetAllBookName(string bookName)
        {
            var res = await _movementService.GetAllFilteredBookNameAsync(bookName);
            if (!res.Success)
                return BadRequest(res);
            return Ok(res);
        }

        [HttpGet("getAllDepositId")]
        public async Task<IActionResult> GetAllDepositId(int id)
        {
            var res = await _movementService.GetAllFilteredDepositIdAsync(id);
            if (!res.Success)
                return BadRequest(res);
            return Ok(res);
        }

        [HttpGet("getAllStudentId")]
        public async Task<IActionResult> GetAllStudentId(int id)
        {
            var res = await _movementService.GetAllFilteredStudentIdAsync(id);
            if (!res.Success)
                return BadRequest(res);
            return Ok(res);
        }

        [HttpGet("getAllStudentName")]
        public async Task<IActionResult> GetAllStudentName(string studentName)
        {
            var res = await _movementService.GetAllFilteredStudentNameAsync(studentName);
            if (!res.Success)
                return BadRequest(res);
            return Ok(res);
        }

        [HttpGet("getAllDonationId")]
        public async Task<IActionResult> GetAllDonationId(int id)
        {
            var res = await _movementService.GetAllFilteredDonationIdAsync(id);
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
