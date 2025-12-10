using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyFinance.Application.UseCases;
using MyFinance.Shared.DTOs;

namespace MyFinance.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionsController : ControllerBase
    {
        private readonly ITransactionUseCase _transactionUseCase;
        public TransactionsController(ITransactionUseCase transactionUseCase)
            => _transactionUseCase = transactionUseCase;

        // GET api/transactions
        // GET api/transactions?startDate=2025-05-01&endDate=2025-05-31&transactionType=Income&description=agua&sortField=Date&sortDesc=true
        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] DateTime? startDate,
            [FromQuery] DateTime? endDate,
            [FromQuery] string? transactionType,
            [FromQuery] string? description,
            [FromQuery] string sortField = "Date",
            [FromQuery] bool sortDesc = false)
        {
            var userIdClaim = User.FindFirst("userId")?.Value;
            Guid.TryParse(userIdClaim, out var userId);
            var list = await _transactionUseCase.GetTransactionsAsync(startDate, endDate, transactionType, description, sortField, sortDesc, userId);
            return Ok(list);
        }
        // GET api/transactions/{id}
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<TransactionDto>> GetById(Guid id)
        {
            var dto = await _transactionUseCase.GetTransactionByIdAsync(id);
            if (dto == null) return NotFound();
            return Ok(dto);
        }
        // Reemplaza la asignación directa de dto.UserId por la creación de un nuevo TransactionDto con el UserId asignado.
        // Sustituye este bloque en el método Create:

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TransactionDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.TipoIngreso) || string.IsNullOrWhiteSpace(dto.OrigenIngreso))
            {
                return BadRequest("Los campos TipoIngreso y OrigenIngreso son obligatorios.");
            }
            // Si usas JWT, puedes obtener el userId así:
            var userIdClaim = User.FindFirst("userId")?.Value;
            TransactionDto dtoWithUser = dto;
            if (Guid.TryParse(userIdClaim, out var userId))
                dtoWithUser = dto with { UserId = userId };

            await _transactionUseCase.AddTransactionAsync(dtoWithUser);
            // devolvemos 201 Created sin ubicación específica
            return CreatedAtAction(nameof(GetAll), null);
        }
        // PUT api/transactions/{id}
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] TransactionDto dto)
        {
            if (id != dto.Id)
                return BadRequest("El Id de la ruta y del cuerpo no coinciden.");

            if (string.IsNullOrWhiteSpace(dto.TipoIngreso) || string.IsNullOrWhiteSpace(dto.OrigenIngreso))
            {
                return BadRequest("Los campos TipoIngreso y OrigenIngreso son obligatorios.");
            }

            var updated = await _transactionUseCase.UpdateTransactionAsync(dto);
            if (!updated)
                return NotFound("Transacción no encontrada.");
            return NoContent();
        }
        // DELETE api/budget/{id}
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _transactionUseCase.DeleteTransactionAsync(id);
            return NoContent();
        }
        // GET api/transactions/summary/2025/5
        [HttpGet("summary/{year:int}/{month:int}")]
        public async Task<IActionResult> GetSummary(int year, int month)
        {
            //Console.WriteLine($"Obteniendo resumen para {year}-{month}");
            var userIdClaim = User.FindFirst("userId")?.Value;
            Guid.TryParse(userIdClaim, out var userId);
            var summary = await _transactionUseCase.GetMonthlySummaryAsync(year, month, userId);
            return Ok(summary);
        }
        [HttpGet("daily-expenses")]
        public async Task<IActionResult> GetDailyExpenses([FromQuery] DateTime start, [FromQuery] DateTime end)
        {
            var userIdClaim = User.FindFirst("userId")?.Value;
            Guid.TryParse(userIdClaim, out var userId);
            if (end < start) return BadRequest("El rango es inválido.");
            var data = await _transactionUseCase.GetDailyExpensesAsync(start, end, userId);
            return Ok(data);
        }
    }
}
