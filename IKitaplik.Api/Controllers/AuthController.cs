using IKitaplik.Api.Services;
using IKitaplik.Business.Abstract;
using IKitaplik.Entities.Concrete;
using IKitaplik.Entities.DTOs.UserDTOs;
using Microsoft.AspNetCore.Mvc;

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
        [HttpGet("login")]
        public IActionResult Login([FromQuery]UserLoginDto userLoginDto)
        {
            var res = _userService.Login(userLoginDto);
            if (!res.Success) return BadRequest(res);

            var token = _jwtService.GenerateToken(res.Data);
            return Ok(new { token = token, res });
        }

        [HttpPost("register")]
        public IActionResult Register(UserRegisterDto userRegisterDto)
        {
            var res = _userService.Register(userRegisterDto);
            if (!res.Success) return BadRequest(res);
            return Ok(res);
        }
    }
}
