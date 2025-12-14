using IKitaplik.Business.Abstract;
using IKitaplik.Entities.DTOs;
using IKitaplik.Entities.DTOs.BookDTOs;
using IKitaplik.Entities.DTOs.DonationDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace IKitaplik.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin,User")]
    public class DonationController : ControllerBase
    {
        private readonly IDonationService _donationService;

        public DonationController(IDonationService donationService)
        {
            _donationService = donationService;
        }

        [HttpPost("add")]
        public async Task<IActionResult> Add([FromBody]DonationAddDto donationAddDto)
        {
            var res = await _donationService.AddAsync(donationAddDto);
            if(!res.Success)
                return BadRequest(res);
            return Ok(res);
        }

        [HttpGet("getAll")]
        public async Task<IActionResult> GetAll(PageRequestDto pageRequestDto)
        {
            var res = await _donationService.GetAllDTOAsync(pageRequestDto.Page,pageRequestDto.PageSize);
            if(!res.Success)
                return BadRequest(res);
            return Ok(res);
        }

        [HttpGet("getById")]
        public async Task<IActionResult> GetById(int id)
        {
            var res = await _donationService.GetByIdDTOAsync(id);
            if(!res.Success)
                return BadRequest(res);
            return Ok(res);
        }
    }
}
