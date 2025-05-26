namespace MyFinance.Application.DTOs
{
    public record TransactionDto(
        Guid Id,
        DateTime Date,
        string Category,
        decimal Amount
    );
}
