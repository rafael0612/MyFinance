using MyFinance.Shared.DTOs;
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
        public async Task<BudgetDto?> GetBudgetByIdAsync(Guid id)
        {
            var dto = await _repo.GetByIdAsync(id);
            return new BudgetDto(
               dto!.Id,
               dto.Year,
               dto.Month,
               dto.Amount,
               dto.AlertThreshold
            );
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
        public async Task<bool> UpdateBudgetAsync(BudgetDto dto)
        {
            // 1) Obtener la entidad desde el repo
            var budget = await _repo.GetByIdAsync(dto.Id);
            if (budget is null)
                // Devolver false para que el Controller responda 404
                return false;
            // 2) Aplicar cambios mediante los mutadores de dominio
            budget.ChangePeriod(dto.Year, dto.Month);
            budget.ChangeAmount(dto.Amount);
            budget.ChangeAlertThreshold(dto.AlertThreshold);
            // 3) Persistir y devolver el resultado del repo
            return await _repo.UpdateAsync(budget);
        }
        public Task DeleteBudgetAsync(Guid id)
            => _repo.DeleteAsync(id);
    }
}