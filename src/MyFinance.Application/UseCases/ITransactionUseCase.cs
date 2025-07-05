//using MyFinance.Application.DTOs;
using MyFinance.Shared;
using MyFinance.Shared.DTOs;
namespace MyFinance.Application.UseCases
{
    public interface ITransactionUseCase
    {
        Task<IEnumerable<TransactionDto>> GetAllTransactionsAsync();
        Task<TransactionDto> GetTransactionByIdAsync(Guid id);
        Task AddTransactionAsync(TransactionDto dto);
        Task<bool> UpdateTransactionAsync(TransactionDto dto);
        Task DeleteTransactionAsync(Guid id);
        Task<MonthlySummaryDto> GetMonthlySummaryAsync(int year, int month);
        Task<IEnumerable<TransactionDto>> GetTransactionsAsync(
            DateTime? startDate,
            DateTime? endDate,
            string? transactionType,
            string? description,
            string sortField,
            bool sortDesc
        );
        Task<IEnumerable<DailyTransactionDto>> GetDailyExpensesAsync(DateTime start, DateTime end);
    }
}