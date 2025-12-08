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
        public AuthController(IUserRegisterUseCase userUseCase)
        {
            _userUseCase = userUseCase;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterDto dto)
        {
            var (success, message) = await _userUseCase.RegisterAsync(dto);
            if (success)
                return Ok(new { message });
            return BadRequest(new { message });
        }
    }
}
