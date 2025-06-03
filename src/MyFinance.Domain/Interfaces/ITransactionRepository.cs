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
        /// Recupera una transacción por su ID.
        /// </sumary>
        Task<Transaction?> GetByIdAsync(Guid transactionId);
        /// <sumary>
        /// Recupera transacciones filtradas por año y mes.
        /// </sumary>
        Task<IEnumerable<Transaction>> GetByMonthAsync(int year, int month);
        /// <summary>
        /// Recupera transacciones filtradas por fecha, categoráa y descripción.
        /// </summary>
        Task<IEnumerable<Transaction>> GetFilteredAsync(
            DateTime? startDate,
            DateTime? endDate,
            string? transactionType,
            string? description,
            string sortField,
            bool sortDesc
        );
    }
}