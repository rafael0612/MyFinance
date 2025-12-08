using MyFinance.Application.UseCases;
using MyFinance.Domain.Entities;
using MyFinance.Domain.ValueObjects;
using MyFinance.Domain.Interfaces;
using MyFinance.Shared.DTOs;

namespace MyFinance.Application.Services
{
    public class FinancialIndicatorUseCase : IFinancialIndicatorUseCase
    {
        private readonly ITransactionRepository _transactionRepository;

        public FinancialIndicatorUseCase(ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }

        public async Task<FinancialSummaryDto> CalculateIndicators(DateTime period)
        {
            var transactions = await _transactionRepository.GetTransactionsByPeriod(period);

            var totalIncome = transactions.Where(t => t.TransactionType == TransactionType.Income).Sum(t => t.Amount);
            var totalExpenses = transactions.Where(t => t.TransactionType == TransactionType.Expense).Sum(t => t.Amount);
            var essentialExpenses = transactions.Where(t => t.TransactionType == TransactionType.Expense && t.NivelNecesidad == "Esencial").Sum(t => t.Amount);
            var debtExpenses = transactions.Where(t => t.TransactionType == TransactionType.Expense && t.TipoIngreso == "Deuda").Sum(t => t.Amount);

            var monthlyBalance = totalIncome - totalExpenses;
            var savingsRate = totalIncome > 0 ? ((totalIncome - totalExpenses) / totalIncome) * 100 : 0;
            var essentialPercentage = totalExpenses > 0 ? (essentialExpenses / totalExpenses) * 100 : 0;
            var nonEssentialPercentage = 100 - essentialPercentage;
            var debtPercentage = totalExpenses > 0 ? (debtExpenses / totalExpenses) * 100 : 0;

            var indicators = new FinancialIndicators(monthlyBalance, savingsRate, essentialPercentage, nonEssentialPercentage, debtPercentage);
            return new FinancialSummaryDto(period, indicators.MonthlyBalance, indicators.MonthlySavingsRate, indicators.EssentialExpensesPercentage, indicators.NonEssentialExpensesPercentage, indicators.DebtExpensesPercentage);
        }
    }
}