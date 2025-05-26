using MyFinance.Application.DTOs;

namespace MyFinance.Application.UseCases
{
    public interface ITransactionUseCase
    {
        Task<IEnumerable<TransactionDto>> GetAllTransactionsAsync();
        Task AddTransactionAsync(TransactionDto dto);
        Task<MonthlySummaryDto> GetMonthlySummaryAsync(int year, int month);
    }
}