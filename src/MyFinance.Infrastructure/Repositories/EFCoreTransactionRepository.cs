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
        public async Task<bool> UpdateAsync(Transaction transaction)
        {
            _context.Transactions.Update(transaction);
            var updated = await _context.SaveChangesAsync();
            return updated > 0;
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
        /// <summary>
        /// Devuelve la transacción por Id o null si no existe.
        /// </summary>
        public async Task<Transaction?> GetByIdAsync(Guid transactionId)
            => await _context.Transactions
                                        .AsNoTracking()
                                        .FirstOrDefaultAsync(t => t.Id == transactionId);
        public async Task<IEnumerable<Transaction>> GetByMonthAsync(int year, int month)
            => await _context.Transactions
                                        .Where(t => t.Date.Year == year && t.Date.Month == month)
                                        .AsNoTracking()
                                        .ToListAsync();
        public async Task<IEnumerable<Transaction>> GetFilteredAsync(
            DateTime? startDate,
            DateTime? endDate,
            string? transactionType,
            string? description,
            string sortField,
            bool sortDesc)
        {
            // 1) Base query
            IQueryable<Transaction> query = _context.Transactions
                                                    .AsNoTracking();
            // 2) Filtros
            if (startDate.HasValue)
                query = query.Where(t => t.Date.Date >= startDate.Value.Date);

            if (endDate.HasValue)
                query = query.Where(t => t.Date.Date <= endDate.Value.Date);

            if (!string.IsNullOrWhiteSpace(transactionType))
                query = query.Where(t => t.TransactionType!.Name == transactionType);

            if (!string.IsNullOrWhiteSpace(description))
                query = query.Where(t =>
                    t.Description != null &&
                    EF.Functions.Like(t.Description, $"%{description}%"));
            // 3) Ordenamiento
            query = (sortField, sortDesc) switch
            {
                ("Date", false) => query.OrderBy(t => t.Date),
                ("Date", true) => query.OrderByDescending(t => t.Date),
                ("TransactionType", false) => query.OrderBy(t => t.TransactionType!.Name),
                ("TransactionType", true) => query.OrderByDescending(t => t.TransactionType!.Name),
                ("Description", false) => query.OrderBy(t => t.Description),
                ("Description", true) => query.OrderByDescending(t => t.Description),
                ("Amount", false) => query.OrderBy(t => t.Amount),
                ("Amount", true) => query.OrderByDescending(t => t.Amount),
                _ => sortDesc
                                            ? query.OrderByDescending(t => t.Date)
                                            : query.OrderBy(t => t.Date)
            };
            // 4) Ejecuta en la base de datos
            return await query.ToListAsync();
        }
    }
}
