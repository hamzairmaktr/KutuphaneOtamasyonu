using IKitaplik.Business.Abstract;
using IKitaplik.Entities.DTOs.DepositDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IKitaplik.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin,User")]
    public class DepositController : ControllerBase
    {
        private readonly IDepositService _depositService;

        public DepositController(IDepositService depositService)
        {
            _depositService = depositService;
        }

        [HttpPost("depositGiven")]
        public IActionResult DepositGiven(DepositAddDto depositAddDto)
        {
            var res = _depositService.DepositGiven(depositAddDto);
            if (!res.Success)
                return BadRequest(res);
            return Ok(res);
        }

        [HttpPost("depositReceived")]
        public IActionResult DepositReceived(DepositUpdateDto depositUpdateDto)
        {
            var res = _depositService.DepositReceived(depositUpdateDto);
            if (!res.Success)
                return BadRequest(res);
            return Ok(res);
        }

        [HttpPost("delete")]
        public IActionResult Delete(int id)
        {
            var res = _depositService.Delete(id);
            if (!res.Success)
                return BadRequest(res);
            return Ok(res);
        }

        [HttpPost("extendDuDate")]
        public IActionResult ExtendDueDate(DepositExtentDueDateDto depositExtentDueDateDto)
        {
            var res = _depositService.ExtendDueDate(depositExtentDueDateDto);
            if (!res.Success)
                return BadRequest(res);
            return Ok(res);
        }

        [HttpGet("getAll")]
        public IActionResult GetAll()
        {
            var res = _depositService.GetAllDTO();
            if (!res.Success)
                return BadRequest(res);
            return Ok(res);
        }

        [HttpGet("getById")]
        public IActionResult GetById(int id)
        {
            var res = _depositService.GetByIdDTO(id);
            if (!res.Success)
                return BadRequest(res);
            return Ok(res);
        }
    }
}
