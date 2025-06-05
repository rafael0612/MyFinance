namespace MyFinance.Shared.DTOs
{
    public record MonthlySummaryDto(
        int Year,
        int Month,
        decimal Income,
        decimal Expense,
        decimal Balance
    );
}