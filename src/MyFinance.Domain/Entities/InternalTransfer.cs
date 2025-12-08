namespace MyFinance.Domain.Entities
{
    public class InternalTransfer
    {
        public Guid Id { get; private set; }
        public Guid FromAccountId { get; private set; } // ID de la cuenta de origen
        public Guid ToAccountId { get; private set; } // ID de la cuenta de destino
        public decimal Amount { get; private set; } // Monto de la transferencia
        public DateTime Date { get; private set; } // Fecha de la transferencia

        private InternalTransfer() { }

        public InternalTransfer(Guid fromAccountId, Guid toAccountId, decimal amount, DateTime date)
        {
            if (fromAccountId == Guid.Empty || toAccountId == Guid.Empty)
                throw new ArgumentException("Los IDs de las cuentas no pueden estar vacíos.");

            if (amount <= 0)
                throw new ArgumentException("El monto debe ser mayor que cero.");

            Id = Guid.NewGuid();
            FromAccountId = fromAccountId;
            ToAccountId = toAccountId;
            Amount = amount;
            Date = date;
        }
    }
}