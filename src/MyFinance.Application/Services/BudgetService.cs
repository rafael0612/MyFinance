using MyFinance.Application.DTOs;
using MyFinance.Application.UseCases;
using MyFinance.Domain.Interfaces;
using DomainEntity = MyFinance.Domain.Entities.Budget;

namespace MyFinance.Application.Services
{
    public class BudgetService : IBudgetUseCase
    {
        private readonly IBudgetRepository _repo;

        public BudgetService(IBudgetRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<BudgetDto>> GetAllBudgetsAsync()
        {
            var list = await _repo.GetAllAsync();
            return list.Select(b => new BudgetDto(
                b.Id,
                b.Year,
                b.Month,
                b.Amount,
                b.AlertThreshold
            ));
        }

        public async Task<BudgetDto?> GetBudgetByMonthAsync(int year, int month)
        {
            var b = await _repo.GetByMonthAsync(year, month);
            if (b is null) return null;

            return new BudgetDto(
                b.Id,
                b.Year,
                b.Month,
                b.Amount,
                b.AlertThreshold
            );
        }

        public async Task AddBudgetAsync(BudgetDto dto)
        {
            var entity = new DomainEntity(
                dto.Year,
                dto.Month,
                dto.Amount,
                dto.AlertThreshold
            );
            await _repo.AddAsync(entity);
        }

        public Task DeleteBudgetAsync(Guid id)
            => _repo.DeleteAsync(id);
    }
}