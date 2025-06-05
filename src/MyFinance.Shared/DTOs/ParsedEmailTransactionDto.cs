namespace MyFinance.Shared.DTOs
{
    /// <summary>
    /// Represents a parsed email transaction.
    /// </summary>
    /// <param name="Date">The date of the transaction.</param>
    /// <param name="Amount">The amount of the transaction.</param>
    /// <param name="Description">An optional description of the transaction.</param>
    /// <param name="Sender">The sender of the email.</param>
    /// <param name="RawEmailId">The raw email identifier.</param>
    ///

    public class ParsedEmailTransactionDto
    {
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public string? Description { get; set; }
        public string? Sender { get; set; }
        public string? RawEmailId { get; set; }
    }
}
