using MyFinance.Shared.DTOs;

namespace MyFinance.Application.UseCases
{
    public interface IEmailTransactionImportUseCase
    {
        Task<List<ParsedEmailTransactionDto>> ImportNewEmailTransactionsAsync();
    }
}