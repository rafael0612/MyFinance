using MyFinance.Application.Services;
using MyFinance.Domain.Entities;
using MyFinance.Domain.ValueObjects;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace MyFinance.Tests.Application.Services
{
    public class FinancialIndicatorServiceTests
    {
        [Fact]
        public void CalculateIndicators_ShouldReturnCorrectIndicators()
        {
            // Arrange
            var mockTransactionRepository = new Mock<ITransactionRepository>();
            var service = new FinancialIndicatorService(mockTransactionRepository.Object);

            var period = new DateTime(2025, 12, 1);
            var transactions = new List<Transaction>
            {
                new Transaction { Type = TransactionType.Income, Amount = 5000, Date = period },
                new Transaction { Type = TransactionType.Expense, Amount = 2000, Date = period, Category = new Category { IsEssential = true } },
                new Transaction { Type = TransactionType.Expense, Amount = 1000, Date = period, Category = new Category { IsEssential = false } },
                new Transaction { Type = TransactionType.Expense, Amount = 500, Date = period, Category = new Category { IsDebt = true } }
            };

            mockTransactionRepository.Setup(repo => repo.GetTransactionsByPeriod(period)).Returns(transactions.AsQueryable());

            // Act
            var result = service.CalculateIndicators(period);

            // Assert
            Assert.Equal(1500, result.Indicators.MonthlyBalance);
            Assert.Equal(30, result.Indicators.MonthlySavingsRate);
            Assert.Equal(66.67m, Math.Round(result.Indicators.EssentialExpensesPercentage, 2));
            Assert.Equal(33.33m, Math.Round(result.Indicators.NonEssentialExpensesPercentage, 2));
            Assert.Equal(16.67m, Math.Round(result.Indicators.DebtExpensesPercentage, 2));
        }
    }
}