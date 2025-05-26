using MyFinance.Domain.ValueObjects;

namespace MyFinance.Domain.Entities
{
   public class Transaction
   {
      public Guid Id { get; private set; }
      public DateTime Date { get; private set; }
      public Category Category { get; private set; }
      public decimal Amount { get; private set; }

      private Transaction() { }

      public Transaction(DateTime date, Category category, decimal amount)
      {
         if (amount <= 0)
            throw new ArgumentException("El monto de ser positivo.", nameof(amount));

         Id = Guid.NewGuid();
         Date = date;
         Category = category ?? throw new ArgumentNullException(nameof(category));
         Amount = amount;
      }
   }
}