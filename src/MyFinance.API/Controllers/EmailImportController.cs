using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyFinance.Application.UseCases;

namespace MyApp.Namespace
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class EmailImportController : ControllerBase
    {
        private readonly IEmailTransactionImportUseCase _importUseCase;
        public EmailImportController(IEmailTransactionImportUseCase importUseCase)
        {
            _importUseCase = importUseCase;
        }
        [HttpPost("import-emails")]
        public async Task<IActionResult> ImportFromEmails()
        {
            try
            {
                var imported = await _importUseCase.ImportNewEmailTransactionsAsync();
                return Ok(new { message = "Transacciones de correo electrónico importadas correctamente", transactionImported = imported });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = ex.Message });
            }
        }
    }
}
