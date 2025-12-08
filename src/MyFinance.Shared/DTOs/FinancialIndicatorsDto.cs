namespace MyFinance.Shared.DTOs
{
    public class FinancialIndicatorsDto
    {
        public decimal MonthlyBalance { get; set; }
        public decimal MonthlySavingsRate { get; set; }
        public decimal EssentialExpensesPercentage { get; set; }
        public decimal NonEssentialExpensesPercentage { get; set; }
        public decimal DebtExpensesPercentage { get; set; }
    }
}