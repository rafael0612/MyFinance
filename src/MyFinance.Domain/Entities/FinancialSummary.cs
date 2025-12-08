using MyFinance.Domain.ValueObjects;

namespace MyFinance.Domain.Entities
{
    public class FinancialSummary
    {
        public int Id { get; set; }
        public DateTime Period { get; set; } // Represents the month/year of the summary
        public FinancialIndicators Indicators { get; set; } // Value Object for financial indicators

        public FinancialSummary(DateTime period, FinancialIndicators indicators)
        {
            Period = period;
            Indicators = indicators;
        }
    }
}