using MyFinance.Domain.Entities;

namespace MyFinance.Domain.Interfaces
{
    public interface ITransactionRepository
    {
        Task AddAsync(Transaction transaction);
        Task<bool> UpdateAsync(Transaction transaction);
        Task DeleteAsync(Guid transactionId);

        /// <sumary>
        /// Recupera todas las transacciones.
        /// </sumary>
        Task<IEnumerable<Transaction>> GetAllAsync();
        /// <sumary>
        /// Recupera una transacci��n por su ID.
        /// </sumary>
        Task<Transaction> GetByIdAsync(Guid transactionId);
        /// <sumary>
        /// Recupera transacciones filtradas por a�o y mes.
        Task<IEnumerable<Transaction>> GetByMonthAsync(int year, int month);
    }
}