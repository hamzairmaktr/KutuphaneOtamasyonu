using IKitaplik.Business.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IKitaplik.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovementController : ControllerBase
    {
        private readonly IMovementService _movementService;
        public MovementController(IMovementService movementService)
        {
            _movementService = movementService;
        }
        [HttpGet("getAll")]
        public IActionResult GetAll()
        {
            var res = _movementService.GetAll();
            if (!res.Success)
                return BadRequest(res);
            return Ok(res);
        }
        [HttpGet("getAllBookId")]
        public IActionResult GetAllBookId(int id)
        {
            var res = _movementService.GetAllFilteredBookId(id);
            if (!res.Success)
                return BadRequest(res);
            return Ok(res);
        }

        [HttpGet("getAllBookName")]
        public IActionResult GetAllBookName(string bookName)
        {
            var res = _movementService.GetAllFilteredBookName(bookName);
            if (!res.Success)
                return BadRequest(res);
            return Ok(res);
        }

        [HttpGet("getAllDepositId")]
        public IActionResult GetAllDepositId(int id)
        {
            var res = _movementService.GetAllFilteredDepositId(id);
            if (!res.Success)
                return BadRequest(res);
            return Ok(res);
        }

        [HttpGet("getAllStudentId")]
        public IActionResult GetAllStudentId(int id)
        {
            var res = _movementService.GetAllFilteredStudentId(id);
            if (!res.Success)
                return BadRequest(res);
            return Ok(res);
        }

        [HttpGet("getAllStudentName")]
        public IActionResult GetAllStudentName(string studentName)
        {
            var res = _movementService.GetAllFilteredStudentName(studentName);
            if (!res.Success)
                return BadRequest(res);
            return Ok(res);
        }

        [HttpGet("getAllDonationId")]
        public IActionResult GetAllDonationId(int id)
        {
            var res = _movementService.GetAllFilteredDonationId(id);
            if (!res.Success)
                return BadRequest(res);
            return Ok(res);
        }

        [HttpGet("getById")]
        public IActionResult GetById(int id)
        {
            var res = _movementService.GetByIdDto(id);
            if (!res.Success)
                return BadRequest(res);
            return Ok(res);
        }
    }
}
