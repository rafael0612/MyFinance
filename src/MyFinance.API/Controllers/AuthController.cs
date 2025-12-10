using Microsoft.AspNetCore.Mvc;
using MyFinance.Application.UseCases;
using MyFinance.Shared.DTOs;

namespace MyFinance.API.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : Controller
    {
        private readonly IUserRegisterUseCase _userUseCase;
        private readonly IUserLoginUseCase _loginUseCase;
        public AuthController(IUserRegisterUseCase userUseCase, IUserLoginUseCase loginUseCase)
        {
            _userUseCase = userUseCase;
            _loginUseCase = loginUseCase;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterDto dto)
        {
            var (success, message) = await _userUseCase.RegisterAsync(dto);
            if (success)
                return Ok(new { message });
            return BadRequest(new { message });
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto dto)
        {
            var result = await _loginUseCase.LoginAsync(dto);
            if (!string.IsNullOrEmpty(result.Token))
                return Ok(result);
            return BadRequest(new { message = result.Message });
        }
    }
}
