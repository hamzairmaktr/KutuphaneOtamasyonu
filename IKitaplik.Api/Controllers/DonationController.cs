using IKitaplik.Business.Abstract;
using IKitaplik.Entities.DTOs.BookDTOs;
using IKitaplik.Entities.DTOs.DonationDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
        public IActionResult Add([FromBody]DonationAddDto donationAddDto)
        {
            var res = _donationService.Add(donationAddDto);
            if(!res.Success)
                return BadRequest(res);
            return Ok(res);
        }

        [HttpGet("getAll")]
        public IActionResult GetAll()
        {
            var res = _donationService.GetAllDTO();
            if(!res.Success)
                return BadRequest(res);
            return Ok(res);
        }

        [HttpGet("getById")]
        public IActionResult GetById(int id)
        {
            var res = _donationService.GetByIdDTO(id);
            if(!res.Success)
                return BadRequest(res);
            return Ok(res);
        }
    }
}
