namespace MyFinance.Domain.ValueObjects
{
    public sealed class Category
    {
        public string Name { get; }
        private Category(string name)
        {
            Name = name;
        }
        public static Category Income => new("Income");
        public static Category Expense => new("Expense");
        public override bool Equals(object? obj) =>
            obj is Category other && Name == other.Name;
        public override int GetHashCode() => Name.GetHashCode();
        public override string ToString() => Name;        
    }
}
