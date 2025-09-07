using IKitaplik.Business.Abstract;
using IKitaplik.Entities.DTOs;
using IKitaplik.Entities.DTOs.ImagesDTOs;
using IKitaplik.Entities.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace IKitaplik.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin,User")]
    public class ImageController : ControllerBase
    {
        private readonly IImageService _imageService;

        public ImageController(IImageService imageService)
        {
            _imageService = imageService;
        }

        [HttpPost("add")]
        public async Task<IActionResult> Upload([FromForm] ImageUploadDto imageUploadDto)
        {
            var res = await _imageService.UploadAsync(imageUploadDto);
            if (res.Success)
            {
                return Ok(res);
            }
            return BadRequest(res);
        }

        [HttpGet("getAll")]
        public async Task<IActionResult> GetAll([FromQuery] ImageType? type = null, int relantshipId = 0)
        {
            var res = await _imageService.GetAllAsync(type, relantshipId);
            if (res.Success)
            {
                return Ok(res);
            }
            return BadRequest(res);
        }

        [HttpGet("getById")]
        public async Task<IActionResult> GetById([FromQuery] int id)
        {
            var res = await _imageService.GetByIdAsync(id);
            if (res.Success)
            {
                return Ok(res);
            }
            return BadRequest(res);
        }

        [HttpPost("delete")] 
        public async Task<IActionResult> Delete([FromBody] DeleteDto deleteDto)
        { 
            var deleted = await _imageService.DeleteAsync(deleteDto.Id); 
            if (deleted.Success) 
                return Ok(deleted);
            return BadRequest(deleted);
        }
    }
}
