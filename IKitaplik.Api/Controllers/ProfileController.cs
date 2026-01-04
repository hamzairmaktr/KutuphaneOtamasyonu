using IKitaplik.Business.Abstract;
using IKitaplik.Entities.DTOs.UserDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IKitaplik.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProfileController : ControllerBase
    {
        private readonly IUserService _userService;

        public ProfileController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetProfile()
        {
            if (!int.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out int userId))
                 return Unauthorized();

            var res = await _userService.GetByIdAsync(userId);
            if (!res.Success) return BadRequest(res);
            return Ok(res);
        }

        [HttpPost("update")]
        public async Task<IActionResult> UpdateProfile([FromBody] UserProfileUpdateDto profileDto)
        {
            if (!int.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out int userId))
                 return Unauthorized();
            
            profileDto.Id = userId; 
            
            var res = await _userService.UpdateProfileAsync(profileDto);
            if (!res.Success) return BadRequest(res);
            return Ok(res);
        }

        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto passwordDto)
        {
            if (!int.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out int userId))
                 return Unauthorized();

            passwordDto.UserId = userId;

            var res = await _userService.ChangePasswordAsync(passwordDto);
            if (!res.Success) return BadRequest(res);
            return Ok(res);
        }
    }
}
