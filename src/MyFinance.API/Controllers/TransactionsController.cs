// MyFinance.API/Controllers/TransactionsController.cs
using Microsoft.AspNetCore.Mvc;
using MyFinance.Application.DTOs;
using MyFinance.Application.UseCases;

namespace MyFinance.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionsController : ControllerBase
    {
        private readonly ITransactionUseCase _transactionUseCase;
        public TransactionsController(ITransactionUseCase transactionUseCase)
            => _transactionUseCase = transactionUseCase;

        // GET api/transactions
        // GET api/transactions?startDate=2025-05-01&endDate=2025-05-31&category=Income&description=agua&sortField=Date&sortDesc=true
        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] DateTime? startDate,
            [FromQuery] DateTime? endDate,
            [FromQuery] string? category,
            [FromQuery] string? description,
            [FromQuery] string sortField = "Date",
            [FromQuery] bool sortDesc = false)
        {
            var list = await _transactionUseCase
            .GetTransactionsAsync(startDate, endDate, category, description, sortField, sortDesc);
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
        // POST api/transactions
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TransactionDto dto)
        {
            await _transactionUseCase.AddTransactionAsync(dto);
            // devolvemos 201 Created sin ubicación específica
            return CreatedAtAction(nameof(GetAll), null);
        }
        // PUT api/transactions/{id}
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] TransactionDto dto)
        {
            if (id != dto.Id)
                return BadRequest("El Id de la ruta y del cuerpo no coinciden.");
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
            Console.WriteLine($"Obteniendo resumen para {year}-{month}");
            var summary = await _transactionUseCase.GetMonthlySummaryAsync(year, month);
            return Ok(summary);
        }
    }
}
