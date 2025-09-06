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
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly JwtService _jwtService;
        public AuthController(IUserService userService, JwtService jwtService)
        {
            _userService = userService;
            _jwtService = jwtService;
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto userLoginDto)
        {
            var res = await _userService.LoginAsync(userLoginDto);
            if (!res.Success)
                return BadRequest(res);

            var accessToken = _jwtService.GenerateToken(res.Data);
            var refreshToken = _jwtService.CreateRefreshToken();
            var setRefreshTokenResponse = await _userService.SetRefreshTokenAsync(refreshToken, DateTime.Now.AddDays(7), res.Data.Id);
            if (!setRefreshTokenResponse.Success)
                return BadRequest(setRefreshTokenResponse);

            return Ok(new { data = new { accessToken = accessToken, refreshToken = refreshToken }, success = res.Success, message = res.Message });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRegisterDto userRegisterDto)
        {
            var res = await _userService.RegisterAsync(userRegisterDto);
            if (!res.Success) return BadRequest(res);
            return Ok(res);
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefeshToken(RefreshTokenDto refreshTokenDto)
        {
            var res = await _userService.GetByRefreshTokenAsync(refreshTokenDto.RefreshToken);
            if (!res.Success)
                return BadRequest(res);
            string accessToken = _jwtService.GenerateToken(res.Data);

            return Ok(new { data = new { accessToken = accessToken, refreshToken = refreshTokenDto.RefreshToken }, message = res.Message, success = true });
        }
    }
}
