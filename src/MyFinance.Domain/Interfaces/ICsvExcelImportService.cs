using MyFinance.Shared.DTOs;

namespace MyFinance.Domain.Interfaces
{

    /// <summary>
    /// Interface for CSV/Excel import service.
    /// </summary>
    /// <remarks>
    /// This interface defines methods for parsing transactions from CSV or Excel files.
    /// </remarks>
    public interface ICsvExcelImportService
    {
        Task<IList<TransactionDto>> ParseTransactionsAsync(Stream fileStream, string extension);
    }
}
