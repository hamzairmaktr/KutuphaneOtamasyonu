using IKitaplik.Api.Services;
using IKitaplik.Business.Abstract;
using IKitaplik.Entities.Concrete;
using IKitaplik.Entities.DTOs.UserDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace IKitaplik.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IUserService userService, JwtService jwtService) : ControllerBase
    {
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto userLoginDto)
        {
            var res = await userService.LoginAsync(userLoginDto);
            if (!res.Success)
                return BadRequest(res);

            var accessToken = jwtService.GenerateToken(res.Data);
            var refreshToken = jwtService.CreateRefreshToken();
            var setRefreshTokenResponse = await userService.SetRefreshTokenAsync(refreshToken, DateTime.Now.AddDays(7), res.Data.Id);
            if (!setRefreshTokenResponse.Success)
                return BadRequest(setRefreshTokenResponse);

            return Ok(new { data = new { accessToken = accessToken, refreshToken = refreshToken }, success = res.Success, message = res.Message });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRegisterDto userRegisterDto)
        {
            var res = await userService.RegisterAsync(userRegisterDto);
            if (!res.Success) return BadRequest(res);
            return Ok(res);
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefeshToken(RefreshTokenDto refreshTokenDto)
        {
            var res = await userService.GetByRefreshTokenAsync(refreshTokenDto.RefreshToken);
            if (!res.Success)
                return BadRequest(res);
            string accessToken = jwtService.GenerateToken(res.Data);

            return Ok(new { data = new { accessToken = accessToken, refreshToken = refreshTokenDto.RefreshToken }, message = res.Message, success = true });
        }
    }
}
