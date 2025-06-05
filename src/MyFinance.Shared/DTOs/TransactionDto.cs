namespace MyFinance.Shared.DTOs
{
    public record TransactionDto(
        Guid Id,
        DateTime Date,
        string TransactionType,
        decimal Amount,
        string? Description = null
    );
}