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
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var list = await _transactionUseCase.GetAllTransactionsAsync();
            return Ok(list);
        }

        // POST api/transactions
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TransactionDto dto)
        {
            await _transactionUseCase.AddTransactionAsync(dto);
            // devolvemos 201 Created sin ubicación específica
            return CreatedAtAction(nameof(GetAll), null);
        }

        // GET api/transactions/summary/2025/5
        [HttpGet("summary/{year:int}/{month:int}")]
        public async Task<IActionResult> GetSummary(int year, int month)
        {
            var summary = await _transactionUseCase.GetMonthlySummaryAsync(year, month);
            return Ok(summary);
        }
    }
}
