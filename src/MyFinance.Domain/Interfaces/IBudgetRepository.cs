using MyFinance.Domain.Entities;

namespace MyFinance.Domain.Interfaces
{
    public interface IBudgetRepository
    {
        Task AddAsync(Budget budget);
        Task UpdateAsync(Budget budget);
        Task DeleteAsync(Guid budgetId);

        ///<sumary>
        /// Recupera el presupuesto para un mes y a�o especificos.
        /// </sumary>
        Task<Budget?> GetByMonthAsync(int year, int month);

        /// <sumary>
        /// Recupera todos los presupuestos resgistrados.
        /// </sumary>
        Task<IEnumerable<Budget>> GetAllAsync();
    }
}