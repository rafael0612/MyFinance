namespace MyFinance.Domain.ValueObjects
{
    public sealed class Category : IEquatable<Category>
    {
        public string Name { get; }
        // Constructor privado para evitar instanciación directa        
        private Category(string name) => Name = name;
        // instancias únicas
        private static readonly Category _income  = new("Income");
        private static readonly Category _expense = new("Expense");
        // Métodos estáticos para crear instancias predefinidas
        public static Category Income => _income;
        public static Category Expense => _expense;
        // public override bool Equals(object? obj) =>
        //     obj is Category other && Name == other.Name;
        public bool Equals(Category? other) =>
            other is not null && Name == other.Name;
        public override bool Equals(object? obj) =>
            Equals(obj as Category);
        public override int GetHashCode() =>
            Name.GetHashCode();
        // operadores de igualdad
        public static bool operator ==(Category? a, Category? b) =>
            a?.Equals(b) ?? b is null;

        public static bool operator !=(Category? a, Category? b) =>
            !(a == b);
        public override string ToString() => Name;        
    }
}
