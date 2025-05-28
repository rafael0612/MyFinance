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
            t.Category!.ToString(),
            t.Amount
          ));
      }
      public async Task<TransactionDto> GetTransactionByIdAsync(Guid id)
      {
         var dto = await _repo.GetByIdAsync(id);
         return new TransactionDto(
            dto.Id,
            dto.Date,
            dto.Category!.ToString(),
            dto.Amount
         );
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
      public async Task<bool> UpdateTransactionAsync(TransactionDto dto)
      {
         var category = dto.Category == Category.Income.ToString() ? Category.Income : Category.Expense;
         // 2) Obtener la entidad desde el repo
         var transaction = await _repo.GetByIdAsync(dto.Id);
         if (transaction is null)
            return false;
         // 3) Aplicar cambios sobre la entidad cargada
         transaction.ChangeDate(dto.Date);
         transaction.ChangeCategory(category);
         transaction.ChangeAmount(dto.Amount);
         // 4) Persistir los cambios
         return await _repo.UpdateAsync(transaction);
      }
      public async Task DeleteTransactionAsync(Guid id)
      {
         await _repo.DeleteAsync(id);
      }
      public async Task<MonthlySummaryDto> GetMonthlySummaryAsync(int year, int month)
      {
         var items = await _repo.GetByMonthAsync(year, month);
         foreach (var item in items)
         {
            Console.WriteLine($"Transaction: {item.Id}, Date: {item.Date}, Category: {item.Category}, Amount: {item.Amount}");
         }

         var income = items.Where(t => t.Category == Category.Income).Sum(t => t.Amount);

         var expense = items.Where(t => t.Category == Category.Expense).Sum(t => t.Amount);
         
         Console.WriteLine($"Income: {income}, Expense: {expense}");
         Console.WriteLine($"Year: {year}, Month: {month}");

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
