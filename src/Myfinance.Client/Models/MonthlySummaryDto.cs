// MyFinance.Client/Models/MonthlySummaryDto.cs
namespace MyFinance.Client.Models
{
    using System;
    /// <summary>
    /// Represents a monthly financial summary.
    /// </summary>
    /// <remarks>
    /// This record is used to transfer monthly summary data between the client and server.
    /// It includes the year, month, total income, total expense, and balance for that month.
    /// </remarks>
    /// <param name="Year">The year of the summary.</param> 
    /// <param name="Month">The month of the summary.</param>
    /// <param name="Income">Total income for the month.</param>
    /// <param name="Expense">Total expense for the month.</param>
    /// <param name="Balance">Balance for the month (Income - Expense).</param>
    // public record MonthlySummaryDto(
    //     int Year,
    //     int Month,
    //     decimal Income,
    //     decimal Expense,
    //     decimal Balance
    // );
    public class MonthlySummaryDto
    {
        public int     Year    { get; set; }
        public int     Month   { get; set; }
        public decimal Income  { get; set; }
        public decimal Expense { get; set; }
        public decimal Balance { get; set; }
    }
}