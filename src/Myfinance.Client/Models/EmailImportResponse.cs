using MyFinance.Shared.DTOs;

namespace MyFinance.Client.Models
{
    public class EmailImportResponse
    {
        public string? Message { get; set; }
        public List<ParsedEmailTransactionDto>? TransaccionesEmail { get; set; }
    }
}