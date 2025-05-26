// MyFinance.Infrastructure/Repositories/EFCoreTransactionRepository.cs
using Microsoft.EntityFrameworkCore;
using MyFinance.Domain.Entities;
using MyFinance.Domain.Interfaces;
using MyFinance.Infrastructure.Data;

namespace MyFinance.Infrastructure.Repositories
{
    public class EFCoreTransactionRepository : ITransactionRepository
    {
        private readonly FinanceDbContext _context;
        public EFCoreTransactionRepository(FinanceDbContext context)
            => _context = context;
        public async Task AddAsync(Transaction transaction)
        {
            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(Transaction transaction)
        {
            _context.Transactions.Update(transaction);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(Guid transactionId)
        {
            var entity = await _context.Transactions.FindAsync(transactionId);
            if (entity is not null)
            {
                _context.Transactions.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<IEnumerable<Transaction>> GetAllAsync()
            => await _context.Transactions
                              .AsNoTracking()
                              .ToListAsync();
        public async Task<IEnumerable<Transaction>> GetByMonthAsync(int year, int month)
            => await _context.Transactions
                              .Where(t => t.Date.Year == year && t.Date.Month == month)
                              .AsNoTracking()
                              .ToListAsync();
    }
}
