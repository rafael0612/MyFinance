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
        // Constructor “nuevo”
        public Budget(int year, int month, decimal amount, decimal alertThreshold = 0.8m)
        {            
            ValidateMonth(month);
            ValidateAmount(amount);
            ValidateThreshold(alertThreshold);

            Id = Guid.NewGuid();
            Year = year;
            Month = month;
            Amount = amount;
            AlertThreshold = alertThreshold;

        }
         // Constructor para rehidratar (por ejemplo en tests o al recrear con id)
        public Budget(Guid id, int year, int month, decimal amount, decimal alertThreshold = 0.8m)
        {
            if (id == Guid.Empty) throw new ArgumentException("El Id no puede estar vacío.", nameof(id));
            ValidateMonth(month);
            ValidateAmount(amount);
            ValidateThreshold(alertThreshold);

            Id = id;
            Year = year;
            Month = month;
            Amount = amount;
            AlertThreshold = alertThreshold;
        }
        // --- MUTADORES ---
        public void ChangePeriod(int year, int month)
        {
            ValidateMonth(month);
            Year = year;
            Month = month;
        }
        public void ChangeAmount(decimal newAmount)
        {
            ValidateAmount(newAmount);
            Amount = newAmount;
        }
        public void ChangeAlertThreshold(decimal newThreshold)
        {
            ValidateThreshold(newThreshold);
            AlertThreshold = newThreshold;
        }
        // --- LÓGICA PRIVADA ---
        private static void ValidateMonth(int month)
        {
            if (month < 1 || month > 12)
                throw new ArgumentOutOfRangeException(nameof(month), "Mes debe estar entre 1 y 12.");
        }
        private static void ValidateAmount(decimal amount)
        {
            if (amount <= 0)
                throw new ArgumentException("El monto del presupuesto debe ser positivo.", nameof(amount));
        }
        private static void ValidateThreshold(decimal threshold)
        {
            if (threshold <= 0 || threshold >= 1)
                throw new ArgumentException("El umbral de alerta debe estar entre 0 y 1.", nameof(threshold));
        }
        public bool IsThresholdExceeded(decimal spent) =>
            spent >= (Amount * AlertThreshold);
    }
}