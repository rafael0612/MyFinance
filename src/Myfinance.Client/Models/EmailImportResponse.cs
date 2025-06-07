using MyFinance.Shared.DTOs;
using System.Text.Json.Serialization;

namespace MyFinance.Client.Models
{
    public class EmailImportResponse
    {
        [JsonPropertyName("message")]
        public string? Message { get; set; }
        [JsonPropertyName("transactionImported")]
        public List<ParsedEmailTransactionDto>? TransactionImported { get; set; }
    }
}