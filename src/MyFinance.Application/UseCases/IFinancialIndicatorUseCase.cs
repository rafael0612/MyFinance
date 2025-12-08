using MyFinance.Shared.DTOs;

namespace MyFinance.Application.UseCases
{
    public interface IFinancialIndicatorUseCase
    {
        Task<FinancialSummaryDto> CalculateIndicators(DateTime period);
    }
}