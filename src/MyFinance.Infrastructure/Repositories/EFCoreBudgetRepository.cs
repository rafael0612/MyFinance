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
        public async Task<bool> UpdateAsync(Budget budget)
        {
            _context.Budgets.Update(budget);
            var updated = await _context.SaveChangesAsync();
            return updated > 0;
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
        public async Task<Budget?> GetByIdAsync(Guid budgetId)
            => await _context.Budgets
                             .AsNoTracking()
                             .FirstOrDefaultAsync(b => b.Id == budgetId);
        public async Task<IEnumerable<Budget>> GetAllAsync(Guid userId)
        {
            var query = _context.Budgets.AsNoTracking().AsQueryable();
            if (userId != Guid.Empty)
                query = query.Where(t => t.UserId == userId);
            query = query.OrderBy(b => b.Year).ThenBy(b => b.Month);
            return await query.ToListAsync();
            //await _context.Budgets
            //                   .AsNoTracking()
            //                   .Where(t => t.UserId == userId)
            //                   .OrderBy(b => b.Year)
            //                   .ThenBy(b => b.Month)
            //                   .ToListAsync();
        }
    }
}
