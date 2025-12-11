using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyFinance.Application.UseCases;
using MyFinance.Shared.DTOs;

namespace MyFinance.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserManagementUseCase _userUseCase;
        public UsersController(IUserManagementUseCase useCase) => _userUseCase = useCase;

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _userUseCase.GetAllAsync());

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var user = await _userUseCase.GetByIdAsync(id);
            return user == null ? NotFound() : Ok(user);
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] UserRegisterDto dto)
        {
            var (success, message) = await _userUseCase.CreateAsync(dto);
            if (success) return Ok(new { message });
            return BadRequest(new { message });
        }

        [HttpPut("{id:guid}/activate")]
        public async Task<IActionResult> Activate(Guid id)
        {
            var result = await _userUseCase.ActivateAsync(id);
            return result ? Ok() : NotFound();
        }

        [HttpPut("{id:guid}/deactivate")]
        public async Task<IActionResult> Deactivate(Guid id)
        {
            var result = await _userUseCase.DeactivateAsync(id);
            return result ? Ok() : NotFound();
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UserDto dto)
        {
            if (id != dto.Id) 
                return BadRequest(new { message = "Id no coincide." });

            var result = await _userUseCase.UpdateAsync(dto);

            if (!result)
                return NotFound(new { message = "Usuario no encontrado." });

            return Ok(new { message = "Usuario actualizado correctamente." });
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _userUseCase.DeleteAsync(id);
            return result ? Ok() : NotFound();
        }
    }
}
