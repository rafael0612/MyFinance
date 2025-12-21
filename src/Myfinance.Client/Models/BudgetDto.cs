// MyFinance.Client/Models/BudgetDto.cs  (por si luego lo necesitas)
namespace MyFinance.Client.Models
{
    using System;

    /// <summary>
    /// Represents a budget for a specific month and year.
    /// </summary>
    /// <param name="Id">Unique identifier for the budget.</param>
    /// <param name="Year">The year of the budget.</param>
    /// <param name="Month">The month of the budget.</param>
    /// <param name="Amount">The total amount allocated for the budget.</param>
    /// <param name="AlertThreshold">The threshold amount for alerts.</param>
    ///
    /// <remarks>
    /// This record is used to transfer budget data between the client and server.
    /// </remarks>
    // public record BudgetDto(
    //     Guid Id,
    //     int Year,
    //     int Month,
    //     decimal Amount,
    //     decimal AlertThreshold
    // );
    public class BudgetDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; } // NUEVO
        public int Year { get; set; }
        public int Month { get; set; }
        public decimal Amount { get; set; }
        public decimal AlertThreshold { get; set; }
    }
}
