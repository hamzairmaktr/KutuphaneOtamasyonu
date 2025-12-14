using IKitaplik.Business.Abstract;
using IKitaplik.Entities.DTOs;
using IKitaplik.Entities.DTOs.DepositDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

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
        public async Task<IActionResult> DepositGiven(DepositAddDto depositAddDto)
        {
            var res = await _depositService.DepositGivenAsync(depositAddDto);
            if (!res.Success)
                return BadRequest(res);
            return Ok(res);
        }

        [HttpPost("depositReceived")]
        public async Task<IActionResult> DepositReceived(DepositUpdateDto depositUpdateDto)
        {
            var res = await _depositService.DepositReceivedAsync(depositUpdateDto);
            if (!res.Success)
                return BadRequest(res);
            return Ok(res);
        }

        [HttpPost("delete")]
        public async Task<IActionResult> Delete(DeleteDto deleteDto)
        {
            var res = await _depositService.DeleteAsync(deleteDto.Id);
            if (!res.Success)
                return BadRequest(res);
            return Ok(res);
        }

        [HttpPost("extendDuDate")]
        public async Task<IActionResult> ExtendDueDate(DepositExtentDueDateDto depositExtentDueDateDto)
        {
            var res = await _depositService.ExtendDueDateAsync(depositExtentDueDateDto);
            if (!res.Success)
                return BadRequest(res);
            return Ok(res);
        }

        [HttpGet("getAll")]
        public async Task<IActionResult> GetAll(PageRequestDto page)
        {
            var res = await _depositService.GetAllDTOAsync(page.Page,page.PageSize);
            if (!res.Success)
                return BadRequest(res);
            return Ok(res);
        }

        [HttpGet("getById")]
        public async Task<IActionResult> GetById(int id)
        {
            var res = await _depositService.GetByIdDTOAsync(id);
            if (!res.Success)
                return BadRequest(res);
            return Ok(res);
        }
    }
}
