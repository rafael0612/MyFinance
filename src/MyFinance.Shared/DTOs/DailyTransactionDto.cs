
// MyFinance.Shared/DTOs/DailyExpenseDto.cs
namespace MyFinance.Shared.DTOs
{
    public class DailyTransactionDto
    {
        public DateTime Date { get; set; }
        public decimal ExpenseAmount { get; set; }
        public decimal IncomeAmount { get; set; }
    }
}
