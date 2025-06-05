using MyFinance.Application.UseCases;
    using MyFinance.Domain.Interfaces;
    using MyFinance.Domain.ValueObjects;
    using MyFinance.Shared.DTOs;
    using DomainEntity = MyFinance.Domain.Entities.Transaction;
namespace MyFinance.Application.Services
{
    /// <summary>
    /// Use case for bulk transaction import.
    /// </summary>
    public class BulkTransactionImportUseCase : IBulkTransactionImportUseCase
    {
        private readonly ICsvExcelImportService _csvExcelImportService;
        private readonly ITransactionRepository _transactionRepository;

        public BulkTransactionImportUseCase(ICsvExcelImportService csvExcelImportService, ITransactionRepository transactionRepository)
        {
            _csvExcelImportService = csvExcelImportService;
            _transactionRepository = transactionRepository;
        }

        /// <inheritdoc />
        public async Task<IList<TransactionDto>> ImportAsync(Stream fileStream, string extension)
        {
            var transactions = await _csvExcelImportService.ParseTransactionsAsync(fileStream, extension);
            // Aquí podrías validar cada transacción, reglas de negocio, etc.
            foreach (var tx in transactions)
            {
                //var transactionType = TransactionType.FromName(tx.TransactionType);
                var transactionType = tx.TransactionType == TransactionType.Income.ToString() ? TransactionType.Income : TransactionType.Expense;
                var entity = new DomainEntity(tx.Date, transactionType, tx.Amount, tx.Description);
                await _transactionRepository.AddAsync(entity); // O mejor aún, un método AddRangeAsync para mayor eficiencia
            }
            return transactions; // Retorna las transacciones importadas
        }
    }
}