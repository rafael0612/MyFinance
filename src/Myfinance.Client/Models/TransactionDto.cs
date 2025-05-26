// MyFinance.Client/Models/TransactionDto.cs
namespace MyFinance.Client.Models
{
    using System;

    /// <summary>
    /// Represents a financial transaction.
    /// /// </summary>
    /// <remarks>
    /// /// This record is used to transfer transaction data between the client and server.
    /// </remarks>
    /// <param name="Id">Unique identifier for the transaction.</param>
    /// <param name="Date">The date of the transaction.</param>
    /// <param name="Category">The category of the transaction (e.g., income, expense).</param>
    /// <param name="Amount">The amount of the transaction.</param>
    // public record TransactionDto(
    //     Guid Id,
    //     DateTime Date,
    //     string Category,
    //     decimal Amount
    // );
    public class TransactionDto
    {
        public Guid    Id       { get; set; }
        public DateTime Date     { get; set; }
        public string  Category { get; set; } = "Income";
        public decimal Amount   { get; set; }
    }
}