using MyFinance.Domain.ValueObjects;

namespace MyFinance.Domain.Entities
{
   public class Transaction
   {
      public Guid Id { get; private set; }
      public DateTime Date { get; private set; }
      public TransactionType? TransactionType { get; private set; }
      public decimal Amount { get; private set; }
      public string? Description { get; private set; }
      public string TipoIngreso { get; private set; } // Activo, Pasivo, Extraordinario, Deuda
      public string OrigenIngreso { get; private set; } // Sueldo, CTS, Préstamo, etc.
      public string? ExpenseCategory { get; private set; } // Alimentación, Transporte, Educación, etc.
      public string NivelNecesidad { get; private set; } // Esencial o NoEsencial
      public string NaturalezaGasto { get; private set; } // Consumo o Financiero
      public bool EsFijo { get; private set; } // Indica si el gasto es fijo o variable

      private Transaction()
      {
          TipoIngreso = string.Empty; // Default value to satisfy non-nullable requirement
          OrigenIngreso = string.Empty; // Default value to satisfy non-nullable requirement
          NivelNecesidad = "Esencial"; // Valor por defecto
          NaturalezaGasto = "Consumo"; // Valor por defecto
      }
      // Constructor usado al crear una transacción nueva
      public Transaction(DateTime date, TransactionType transactionType, decimal amount, string tipoIngreso, string origenIngreso, string? descripcion = null, string? expenseCategory = null, string nivelNecesidad = "Esencial", string naturalezaGasto = "Consumo", bool esFijo = false)
      {
         ValidateAmount(amount);
         Id = Guid.NewGuid();
         Date = date;
         TransactionType = transactionType ?? throw new ArgumentNullException(nameof(transactionType));
         Amount = amount;
         TipoIngreso = tipoIngreso ?? throw new ArgumentNullException(nameof(tipoIngreso));
         OrigenIngreso = origenIngreso ?? throw new ArgumentNullException(nameof(origenIngreso));
         Description = descripcion;
         ExpenseCategory = expenseCategory; // Puede ser nulo
         NivelNecesidad = nivelNecesidad;
         NaturalezaGasto = naturalezaGasto;
         EsFijo = esFijo;
      }
      // Constructor usado para rehidratar (por ejemplo en Update)
      public Transaction(Guid id, DateTime date, TransactionType transactionType, decimal amount, string tipoIngreso, string origenIngreso, string? descripcion = null, string? expenseCategory = null, string nivelNecesidad = "Esencial", string naturalezaGasto = "Consumo", bool esFijo = false)
      {
         if (id == Guid.Empty)
            throw new ArgumentException("El Id no puede estar vacío.", nameof(id));

         ValidateAmount(amount);
         Id = id;
         Date = date;
         TransactionType = transactionType ?? throw new ArgumentNullException(nameof(transactionType));
         Amount = amount;
         TipoIngreso = tipoIngreso ?? throw new ArgumentNullException(nameof(tipoIngreso));
         OrigenIngreso = origenIngreso ?? throw new ArgumentNullException(nameof(origenIngreso));
         Description = descripcion;
         ExpenseCategory = expenseCategory; // Puede ser nulo
         NivelNecesidad = nivelNecesidad;
         NaturalezaGasto = naturalezaGasto;
         EsFijo = esFijo;
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
      public void ChangeTipoIngreso(string newTipoIngreso)
      {
         TipoIngreso = newTipoIngreso ?? throw new ArgumentNullException(nameof(newTipoIngreso));
      }
      public void ChangeOrigenIngreso(string newOrigenIngreso)
      {
         OrigenIngreso = newOrigenIngreso ?? throw new ArgumentNullException(nameof(newOrigenIngreso));
      }
      public void ChangeExpenseCategory(string? newExpenseCategory)
      {
          ExpenseCategory = newExpenseCategory; // Permitir nulo para borrar la categoría
      }
      public void ChangeNivelNecesidad(string newNivelNecesidad)
      {
          NivelNecesidad = newNivelNecesidad ?? throw new ArgumentNullException(nameof(newNivelNecesidad));
      }
      public void ChangeNaturalezaGasto(string newNaturalezaGasto)
      {
          NaturalezaGasto = newNaturalezaGasto ?? throw new ArgumentNullException(nameof(newNaturalezaGasto));
      }
      public void ChangeEsFijo(bool newEsFijo)
      {
          EsFijo = newEsFijo;
      }
      // Lógica de validación
      private static void ValidateAmount(decimal amount)
      {
         if (amount <= 0)
            throw new ArgumentException("El monto debe ser mayor que cero.", nameof(amount));
      }
   }
}