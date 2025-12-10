// API/Controllers/BulkImportController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyFinance.Application.UseCases;

namespace MyFinance.API.Controllers
{
    [Authorize]
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

            //var extension = Path.GetExtension(file.FileName);
            //using var stream = file.OpenReadStream();
            //await _bulkTransactionImportUseCase.ImportAsync(stream, extension);
            //return Ok(new { message = "Transacciones importadas correctamente" });

            try
            {
                var extension = Path.GetExtension(file.FileName);
                using var stream = file.OpenReadStream();
                var imported = await _bulkTransactionImportUseCase.ImportAsync(stream, extension);
                return Ok(new { message = $"Transacciones importadas correctamente. Se importaron {imported.Count}", transactionImported = imported });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = ex.Message });
            }
        }
    }
}
