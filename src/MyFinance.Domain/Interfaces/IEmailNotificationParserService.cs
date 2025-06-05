using MyFinance.Shared.DTOs;

namespace MyFinance.Domain.Interfaces
{
    public interface IEmailNotificationParserService
    {
        Task<IList<ParsedEmailTransactionDto>> ParseNewTransactionsAsync();
    }
}