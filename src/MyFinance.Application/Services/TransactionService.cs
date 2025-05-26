using MyFinance.Application.DTOs;
using MyFinance.Application.UseCases;
using MyFinance.Domain.Interfaces;
using MyFinance.Domain.ValueObjects;
using DomainEntity = MyFinance.Domain.Entities.Transaction;

namespace MyFinance.Application.Services
{
   public class TransactionService : ITransactionUseCase
   {
      private readonly ITransactionRepository _repo;

      public TransactionService(ITransactionRepository repo)
      {
         _repo = repo;
      }

      public async Task<IEnumerable<TransactionDto>> GetAllTransactionsAsync()
      {
         var list = await _repo.GetAllAsync();
         return list.Select(t => new TransactionDto(
            t.Id,
            t.Date,
            t.Category.ToString(),
            t.Amount
          ));
      }

      public async Task AddTransactionAsync(TransactionDto dto)
      {
         var category = dto.Category == Category.Income.ToString() ? Category.Income : Category.Expense;

         var entity = new DomainEntity(
            dto.Date,
            category,
            dto.Amount
         );

         await _repo.AddAsync(entity);
      }

      public async Task<MonthlySummaryDto> GetMonthlySummaryAsync(int year, int month)
      {
         var items = await _repo.GetByMonthAsync(year, month);

         var income = items.Where(t => t.Category == Category.Income).Sum(t => t.Amount);

         var expense = items.Where(t => t.Category == Category.Expense).Sum(t => t.Amount);

         return new MonthlySummaryDto(
            year,
            month,
            income,
            expense,
            income - expense
         );
      }
   }
}
