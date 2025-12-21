namespace MyFinance.Shared.DTOs
{
    public record BudgetDto(
        Guid Id,
        Guid UserId, // NUEVO
        int Year,
        int Month,
        decimal Amount,
        decimal AlertThreshold
    );
}