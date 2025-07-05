// MyFinance.Client/Models/DailyExpenseViewDto.cs
namespace MyFinance.Client.Models
{
    public class DailyTransactionViewDto
    {
        public DateTime Date { get; set; }
        public decimal ExpenseAmount { get; set; }
        public decimal IncomeAmount { get; set; }
    }
}
