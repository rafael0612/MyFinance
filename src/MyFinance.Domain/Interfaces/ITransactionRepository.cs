using MyFinance.Domain.Entities;

namespace MyFinance.Domain.Interfaces
{
    public interface ITransactionRepository
    {
        Task AddAsync(Transaction transaction);
        Task UpdateAsync(Transaction transaction);
        Task DeleteAsync(Guid transactionId);

        /// <sumary>
        /// Recupera todas las transacciones.
        /// </sumary>
        Task<IEnumerable<Transaction>> GetAllAsync();

        /// <sumary>
        /// Recupera transacciones filtradas por a�o y mes.
        Task<IEnumerable<Transaction>> GetByMonthAsync(int year, int month);
    }
}