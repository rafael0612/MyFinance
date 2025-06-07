using MyFinance.Client.Models;
using System.Text.Json.Serialization;

namespace Myfinance.Client.Models
{
    public class TransactionsImportResponse
    {
        [JsonPropertyName("message")]
        public string Message { get; set; } = default!;
        [JsonPropertyName("transactionImported")]
        public List<TransactionDto> TransactionsImported { get; set; } = new();
    }
}
