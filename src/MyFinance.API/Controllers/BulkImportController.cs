// API/Controllers/BulkImportController.cs
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyFinance.Application.UseCases;

namespace MyFinance.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BulkImportController : ControllerBase
    {
        private readonly IBulkTransactionImportUseCase _bulkTransactionImportUseCase;

        public BulkImportController(IBulkTransactionImportUseCase bulkTransactionImportUseCase)
        {
            _bulkTransactionImportUseCase = bulkTransactionImportUseCase;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (file == null || file.Length == 0)
                    return BadRequest("Archivo no válido");

            var extension = Path.GetExtension(file.FileName);
            using var stream = file.OpenReadStream();
            await _bulkTransactionImportUseCase.ImportAsync(stream, extension);
            return Ok(new { message = "Transacciones importadas correctamente" });
        }
    }
}
