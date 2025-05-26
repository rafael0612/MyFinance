namespace MyFinance.Domain.Entities
{
    public class Budget
    {
        public Guid Id { get; private set; }
        public int Year { get; private set; }
        public int Month { get; private set; }
        public decimal Amount { get; private set; }
        public decimal AlertThreshold { get; private set; }

        private Budget() { }

        public Budget(int year, int month, decimal amount, decimal alertThreshold = 0.8m)
        {
            if (month < 1 || month > 12)
                throw new ArgumentOutOfRangeException(nameof(month), "Mes debe estar entre 1 y 12.");
            if (amount <= 0)
                throw new ArgumentException("El monto del presupuesto debe ser positivo", nameof(amount));
            if (alertThreshold <= 0 || alertThreshold >= 1)
                throw new ArgumentException("El umbral de alerta debe estar entre 0 y 1.", nameof(alertThreshold));

            Id = Guid.NewGuid();
            Year = year;
            Month = month;
            Amount = amount;
            AlertThreshold = alertThreshold;

        }

        public bool IsThresholdExceeded(decimal spent) =>
            spent >= (Amount * AlertThreshold);
    }
}