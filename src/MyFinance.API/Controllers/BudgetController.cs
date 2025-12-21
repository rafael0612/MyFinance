// MyFinance.API/Controllers/BudgetController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyFinance.Application.UseCases;
using MyFinance.Shared.DTOs;

namespace MyFinance.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class BudgetController : ControllerBase
    {
        private readonly IBudgetUseCase _budgetUseCase;
        public BudgetController(IBudgetUseCase budgetUseCase)
            => _budgetUseCase = budgetUseCase;

        // GET api/budget
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var userIdClaim = User.FindFirst("userId")?.Value;
            var userTypeClaim = User.FindFirst("userType")?.Value;
            Guid.TryParse(userIdClaim, out var userId);
            bool isAdmin = userTypeClaim == "Admin";

            var list = await _budgetUseCase.GetAllBudgetsAsync(isAdmin ? Guid.Empty : userId);
            return Ok(list);
        }
        // GET api/budget/{id}
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<BudgetDto>> GetById(Guid id)
        {
            var dto = await _budgetUseCase.GetBudgetByIdAsync(id);
            if (dto == null) return NotFound();
            return Ok(dto);
        }
        // GET api/budget/2025/5
        [HttpGet("{year:int}/{month:int}")]
        public async Task<IActionResult> GetByMonth(int year, int month)
        {
            //var userIdClaim = User.FindFirst("userId")?.Value;
            //Guid.TryParse(userIdClaim, out var userId);
            var budget = await _budgetUseCase.GetBudgetByMonthAsync(year, month);
            if (budget == null) return NotFound("Presupuesto no encontrado para el mes y año especificados.");
            return Ok(budget);
        }
        // POST api/budget
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] BudgetDto dto)
        {
            await _budgetUseCase.AddBudgetAsync(dto);
            return CreatedAtAction(nameof(GetByMonth), new { dto.Year, dto.Month }, dto);
        }
        // PUT api/budget/{id}
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] BudgetDto dto)
        {
            if (id != dto.Id)
                return BadRequest("El Id de la ruta y del cuerpo no coinciden.");
            var updated = await _budgetUseCase.UpdateBudgetAsync(dto);
            if (!updated)
                return NotFound("Presupuesto no encontrado.");
            return NoContent();
        }
        // DELETE api/budget/{id}
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _budgetUseCase.DeleteBudgetAsync(id);
            return NoContent();
        }
    }
}