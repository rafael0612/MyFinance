using MyFinance.Shared.DTOs;

namespace MyFinance.Application.UseCases
{
    public interface IUserRegisterUseCase
    {
        Task<(bool Success, string Message)> RegisterAsync(UserRegisterDto dto);
    }
}
