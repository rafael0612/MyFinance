namespace MyFinance.Shared.DTOs
{
    public record TransactionDto(
        Guid Id,
        DateTime Date,
        string TransactionType,
        decimal Amount,
        string TipoIngreso = "Activo",         // Clasificación del ingreso
        string OrigenIngreso = "Es Gasto",       // Origen del ingreso
        string? Description = null,
        string ExpenseCategory = "", // Categoría del gasto
        bool EsFijo = false,         // Indica si el gasto es fijo
        string NaturalezaGasto = "Consumo", // Naturaleza del gasto
        string NivelNecesidad = "Esencial"  // Nivel de necesidad del gasto
    );
}