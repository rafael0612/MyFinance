namespace MyFinance.Shared.DTOs
{
    public record BudgetDto(
        Guid Id,
        int Year,
        int Month,
        decimal Amount,
        decimal AlertThreshold
    );
}