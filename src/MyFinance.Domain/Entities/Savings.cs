namespace MyFinance.Domain.Entities
{
    public class Savings
    {
        public Guid Id { get; private set; }
        public string? Name { get; private set; } // Nombre del ahorro (por ejemplo, "Fondo de emergencia")
        public decimal Balance { get; private set; } // Saldo actual del ahorro

        private Savings() { }

        public Savings(string name, decimal initialBalance)
        {
            Id = Guid.NewGuid();
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Balance = initialBalance >= 0 ? initialBalance : throw new ArgumentException("El saldo inicial no puede ser negativo.");
        }

        public void Deposit(decimal amount)
        {
            if (amount <= 0)
                throw new ArgumentException("El monto debe ser mayor que cero.");

            Balance += amount;
        }

        public void Withdraw(decimal amount)
        {
            if (amount <= 0)
                throw new ArgumentException("El monto debe ser mayor que cero.");

            if (amount > Balance)
                throw new InvalidOperationException("No hay suficiente saldo para realizar el retiro.");

            Balance -= amount;
        }
    }
}