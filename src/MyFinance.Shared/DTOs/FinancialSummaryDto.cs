
namespace MyFinance.Shared.DTOs
{
    public class FinancialSummaryDto
    {
        public DateTime Period { get; set; }
        public decimal MonthlyBalance { get; set; } // Saldo mensual: Ingresos - Gastos
        public decimal MonthlySavingsRate { get; set; } // Tasa de ahorro mensual
        public decimal EssentialExpensesPercentage { get; set; } // % de gastos esenciales
        public decimal NonEssentialExpensesPercentage { get; set; } // % de gastos no esenciales
        public decimal DebtExpensesPercentage { get; set; } // % de gastos en deudas

        public FinancialSummaryDto(
            DateTime period,
            decimal monthlyBalance,
            decimal monthlySavingsRate,
            decimal essentialExpensesPercentage,
            decimal nonEssentialExpensesPercentage,
            decimal debtExpensesPercentage)
        {
            Period = period;
            MonthlyBalance = monthlyBalance;
            MonthlySavingsRate = monthlySavingsRate;
            EssentialExpensesPercentage = essentialExpensesPercentage;
            NonEssentialExpensesPercentage = nonEssentialExpensesPercentage;
            DebtExpensesPercentage = debtExpensesPercentage;
        }
    }
}