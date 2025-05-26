// MyFinance.Infrastructure/Repositories/EFCoreBudgetRepository.cs
using Microsoft.EntityFrameworkCore;
using MyFinance.Domain.Entities;
using MyFinance.Domain.Interfaces;
using MyFinance.Infrastructure.Data;

namespace MyFinance.Infrastructure.Repositories
{
    public class EFCoreBudgetRepository : IBudgetRepository
    {
        private readonly FinanceDbContext _context;

        public EFCoreBudgetRepository(FinanceDbContext context)
            => _context = context;

        public async Task AddAsync(Budget budget)
        {
            _context.Budgets.Add(budget);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Budget budget)
        {
            _context.Budgets.Update(budget);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid budgetId)
        {
            var entity = await _context.Budgets.FindAsync(budgetId);
            if (entity is not null)
            {
                _context.Budgets.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Budget?> GetByMonthAsync(int year, int month)
            => await _context.Budgets
                             .AsNoTracking()
                             .FirstOrDefaultAsync(b => b.Year == year && b.Month == month);

        public async Task<IEnumerable<Budget>> GetAllAsync()
            => await _context.Budgets
                             .AsNoTracking()
                             .ToListAsync();
    }
}
