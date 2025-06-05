using MyFinance.Shared.DTOs;
namespace MyFinance.Application.UseCases
{
    /// <summary>
    /// Interface for bulk transaction import use case.
    /// </summary>
    /// <remarks>
    /// This interface defines methods for importing transactions in bulk from CSV or Excel files.
    /// </remarks>
    public interface IBulkTransactionImportUseCase
    {
        /// <summary>
        /// Imports transactions from a file stream.
        /// </summary>
        /// <param name="fileStream">The stream of the file to import.</param>
        /// <param name="extension">The file extension (e.g., .csv, .xlsx).</param>
        /// <returns>A list of imported transaction DTOs.</returns>
        Task<IList<TransactionDto>> ImportAsync(Stream fileStream, string extension);
    }
}
