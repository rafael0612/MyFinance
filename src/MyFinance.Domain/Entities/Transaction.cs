using MyFinance.Domain.ValueObjects;

namespace MyFinance.Domain.Entities
{
   public class Transaction
   {
      public Guid Id { get; private set; }
      public DateTime Date { get; private set; }
      public Category? Category { get; private set; }
      public decimal Amount { get; private set; }

      private Transaction() { }
      // Constructor usado al crear una transacción nueva
      public Transaction(DateTime date, Category category, decimal amount)
      {
         ValidateAmount(amount);
         Id = Guid.NewGuid();
         Date = date;
         Category = category ?? throw new ArgumentNullException(nameof(category));
         Amount = amount;
      }
      // Constructor usado para rehidratar (por ejemplo en Update)
      public Transaction(Guid id, DateTime date, Category category, decimal amount)
      {
         if (id == Guid.Empty)
            throw new ArgumentException("El Id no puede estar vacío.", nameof(id));

         ValidateAmount(amount);
         Id = id;
         Date = date;
         Category = category ?? throw new ArgumentNullException(nameof(category));
         Amount = amount;
      }
      // Métodos de agregado para cambiar el estado
      public void ChangeDate(DateTime newDate)
      {
         Date = newDate;
      }
      public void ChangeCategory(Category newCategory)
      {
         Category = newCategory ?? throw new ArgumentNullException(nameof(newCategory));
      }
      public void ChangeAmount(decimal newAmount)
      {
         ValidateAmount(newAmount);
         Amount = newAmount;
      }
      // Lógica de validación
      private static void ValidateAmount(decimal amount)
      {
         if (amount <= 0)
            throw new ArgumentException("El monto debe ser mayor que cero.", nameof(amount));
      }
   }
}