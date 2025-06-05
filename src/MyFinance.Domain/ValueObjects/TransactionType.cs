namespace MyFinance.Domain.ValueObjects
{
    public sealed class TransactionType : IEquatable<TransactionType>
    {
        public string Name { get; }
        // Constructor privado para evitar instanciación directa        
        private TransactionType(string name) => Name = name;
        // instancias únicas
        private static readonly TransactionType _income  = new("Income");
        private static readonly TransactionType _expense = new("Expense");
        // Métodos estáticos para crear instancias predefinidas
        public static TransactionType Income => _income;
        public static TransactionType Expense => _expense;
        // Método de fábrica para reconstruir desde string
        public static TransactionType FromName(string name)
        {
            return name switch
            {
                "Income" => Income,
                "Expense" => Expense,
                _ => new TransactionType(name)
            };
        }
        // public override bool Equals(object? obj) =>
        //     obj is TransactionType other && Name == other.Name;
        public bool Equals(TransactionType? other) =>
            other is not null && Name == other.Name;
        public override bool Equals(object? obj) =>
            Equals(obj as TransactionType);
        public override int GetHashCode() =>
            Name.GetHashCode();
        // operadores de igualdad
        public static bool operator ==(TransactionType? a, TransactionType? b) =>
            a?.Equals(b) ?? b is null;

        public static bool operator !=(TransactionType? a, TransactionType? b) =>
            !(a == b);
        public override string ToString() => Name;        
    }
}
