using Microsoft.AspNetCore.Mvc;
using MyFinance.Application.UseCases;
using MyFinance.Shared.DTOs;
using System;
using System.Threading.Tasks;

namespace MyFinance.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FinancialIndicatorsController : ControllerBase
    {
        private readonly IFinancialIndicatorUseCase _financialIndicatorService;

        public FinancialIndicatorsController(IFinancialIndicatorUseCase financialIndicatorService)
        {
            _financialIndicatorService = financialIndicatorService;
        }

        [HttpGet("{year}/{month}")]
        public async Task<ActionResult<FinancialIndicatorsDto>> GetFinancialIndicators(int year, int month)
        {
            try
            {
                var period = new DateTime(year, month, 1);
                var financialSummary = await _financialIndicatorService.CalculateIndicators(period);

                var dto = new FinancialIndicatorsDto
                {
                    MonthlyBalance = financialSummary.MonthlyBalance,
                    MonthlySavingsRate = financialSummary.MonthlySavingsRate,
                    EssentialExpensesPercentage = financialSummary.EssentialExpensesPercentage,
                    NonEssentialExpensesPercentage = financialSummary.NonEssentialExpensesPercentage,
                    DebtExpensesPercentage = financialSummary.DebtExpensesPercentage
                };

                return Ok(dto);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}