//using MyFinance.Application.DTOs;
using MyFinance.Shared.DTOs;
using MyFinance.Application.UseCases;
using MyFinance.Domain.Interfaces;
using MyFinance.Domain.ValueObjects;
using DomainEntity = MyFinance.Domain.Entities.Transaction;
using MyFinance.Shared;

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
            t.TransactionType!.ToString(),
            t.Amount,
            t.Description
         ));
      }
      public async Task<TransactionDto> GetTransactionByIdAsync(Guid id)
      {
         var dto = await _repo.GetByIdAsync(id);
         return new TransactionDto(
            dto!.Id,
            dto.Date,
            dto.TransactionType!.ToString(),
            dto.Amount,
            dto.Description
         );
      }
      public async Task AddTransactionAsync(TransactionDto dto)
      {
         var transactionType = dto.TransactionType == TransactionType.Income.ToString() ? TransactionType.Income : TransactionType.Expense;

         var entity = new DomainEntity(
            dto.Date,
            transactionType,
            dto.Amount,
            dto.Description
         );

         await _repo.AddAsync(entity);
      }
      public async Task<bool> UpdateTransactionAsync(TransactionDto dto)
      {
         var transactionType = dto.TransactionType == TransactionType.Income.ToString() ? TransactionType.Income : TransactionType.Expense;
         // 2) Obtener la entidad desde el repo
         var transaction = await _repo.GetByIdAsync(dto.Id);
         if (transaction is null)
            return false;
         // 3) Aplicar cambios sobre la entidad cargada
         transaction.ChangeDate(dto.Date);
         transaction.ChangeTransactionType(transactionType);
         transaction.ChangeAmount(dto.Amount);
         transaction.ChangeDescription(dto.Description);
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
            Console.WriteLine($"Transaction: {item.Id}, Date: {item.Date}, Category: {item.TransactionType}, Amount: {item.Amount}");
         }

         var income = items.Where(t => t.TransactionType == TransactionType.Income).Sum(t => t.Amount);

         var expense = items.Where(t => t.TransactionType == TransactionType.Expense).Sum(t => t.Amount);

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
      /// <summary>
      /// Devuelve transacciones aplicando filtros de fecha, categoría y descripción,
      /// y luego ordenándolas según campo y sentido.
      /// </summary>
      public async Task<IEnumerable<TransactionDto>> GetTransactionsAsync(
            DateTime? startDate,
            DateTime? endDate,
            string? category,
            string? transactionType,
            string sortField,
            bool sortDesc
      )
      {
         // 1) Obtiene todas (o podrías tener un método de repo que acepte filtros)
         // Llamas al nuevo método del repo
         var items = await _repo.GetFilteredAsync(
            startDate, endDate, category, transactionType, sortField, sortDesc
         );
         // Proyectas a DTO
         return items.Select(t => new TransactionDto(
            t.Id,
            t.Date,
            t.TransactionType!.ToString(),
            t.Amount,
            t.Description
         ));
      }
      /// <summary>
      /// Obtiene las transacciones por rango de fechas.
      /// </summary>
      /// <param name="start">Fecha de inicio.</param>
      ///  <param name="end">Fecha de fin.</param>
      /// <returns>Lista de transacciones.</returns>
      /// <remarks>
      public async Task<IEnumerable<DailyTransactionDto>> GetDailyExpensesAsync(DateTime start, DateTime end)
      {
         // 1) Trae todas las transacciones en rango
         var txs = await _repo.GetByDateRangeAsync(start, end);

         // 2) Filtra sólo Expense y agrupa por día
         var exp = txs
            .Where(t => t.TransactionType == TransactionType.Expense)
            .GroupBy(t => t.Date.Date)
            .ToDictionary(g => g.Key, g => g.Sum(t => t.Amount));
         var inc = txs
            .Where(t => t.TransactionType == TransactionType.Income)
            .GroupBy(t => t.Date.Date)
            .ToDictionary(g => g.Key, g => g.Sum(t => t.Amount));

         // 3) Genera la lista de fechas desde start a end
         var list = new List<DailyTransactionDto>();
         for (var day = start.Date; day <= end.Date; day = day.AddDays(1))
         {
            list.Add(new DailyTransactionDto
            {
               Date = day,
               ExpenseAmount = exp.TryGetValue(day, out var sum) ? sum : 0m,
               IncomeAmount = inc.TryGetValue(day, out var incSum) ? incSum : 0m
            });
         }

         return list;
      }
   }
}
