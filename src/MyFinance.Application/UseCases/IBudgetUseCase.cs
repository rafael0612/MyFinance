using MyFinance.Application.DTOs;

namespace MyFinance.Application.UseCases
{
    public interface IBudgetUseCase
    {
        Task<IEnumerable<BudgetDto>> GetAllBudgetsAsync();
        Task<BudgetDto?> GetBudgetByMonthAsync(int year, int month);
        Task AddBudgetAsync(BudgetDto dto);
        Task DeleteBudgetAsync(Guid id);
    }
}