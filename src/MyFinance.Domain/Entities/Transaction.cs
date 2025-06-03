using MyFinance.Domain.ValueObjects;

namespace MyFinance.Domain.Entities
{
   public class Transaction
   {
      public Guid Id { get; private set; }
      public DateTime Date { get; private set; }
      public TransactionType? TransactionType { get; private set; }
      public decimal Amount { get; private set; }
      public string? Description { get; set; }

      private Transaction() { }
      // Constructor usado al crear una transacción nueva
      public Transaction(DateTime date, TransactionType transactionType, decimal amount, string? descripcion = null)
      {
         ValidateAmount(amount);
         Id = Guid.NewGuid();
         Date = date;
         TransactionType = transactionType ?? throw new ArgumentNullException(nameof(transactionType));
         Amount = amount;
         Description = descripcion;
      }
      // Constructor usado para rehidratar (por ejemplo en Update)
      public Transaction(Guid id, DateTime date, TransactionType transactionType, decimal amount, string? descripcion = null)
      {
         if (id == Guid.Empty)
            throw new ArgumentException("El Id no puede estar vacío.", nameof(id));

         ValidateAmount(amount);
         Id = id;
         Date = date;
         TransactionType = transactionType ?? throw new ArgumentNullException(nameof(transactionType));
         Amount = amount;
         Description = descripcion;
      }
      // Métodos de agregado para cambiar el estado
      public void ChangeDate(DateTime newDate)
      {
         Date = newDate;
      }
      public void ChangeTransactionType(TransactionType newTransactionType)
      {
         TransactionType = newTransactionType ?? throw new ArgumentNullException(nameof(newTransactionType));
      }
      public void ChangeAmount(decimal newAmount)
      {
         ValidateAmount(newAmount);
         Amount = newAmount;
      }
      public void ChangeDescription(string? newDescription)
      {
         Description = newDescription;
      }
      // Lógica de validación
      private static void ValidateAmount(decimal amount)
      {
         if (amount <= 0)
            throw new ArgumentException("El monto debe ser mayor que cero.", nameof(amount));
      }
   }
}