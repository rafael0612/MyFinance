using MyFinance.Application.UseCases;
using MyFinance.Domain.Entities;
using MyFinance.Domain.Interfaces;
using MyFinance.Domain.ValueObjects;
using MyFinance.Shared.DTOs;
using DomainEntity = MyFinance.Domain.Entities.Transaction;

namespace MyFinance.Application.Services
{
    public class EmailTransactionImportUseCase : IEmailTransactionImportUseCase
    {
        private readonly IEmailNotificationParserService _emailParserService;
        private readonly ITransactionRepository _transactionRepo;
        public EmailTransactionImportUseCase(IEmailNotificationParserService emailParserService, ITransactionRepository transactionRepo)
        {
            _emailParserService = emailParserService;
            _transactionRepo = transactionRepo;
        }
        public async Task<List<ParsedEmailTransactionDto>> ImportNewEmailTransactionsAsync()
        {
            var emails = await _emailParserService.ParseNewTransactionsAsync();
            var emailsProcessed = new List<ParsedEmailTransactionDto>();
            foreach (var dto in emails)
            {
                var transactionType = dto.Amount < 0 ? TransactionType.Expense : TransactionType.Income;
                var entity = new DomainEntity(
                    dto.Date,
                    transactionType,
                    dto.Amount,
                    "Activo", // Valor predeterminado para TipoIngreso
                    "Importación por correo", // Valor predeterminado para OrigenIngreso
                    dto.Description
                );
                await _transactionRepo.AddAsync(entity);
                emailsProcessed.Add(dto);
            }
            return emailsProcessed.ToList();
        }
    }
}