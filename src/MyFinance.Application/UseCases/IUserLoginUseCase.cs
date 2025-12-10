using MyFinance.Shared.DTOs;

namespace MyFinance.Application.UseCases
{
    public interface IUserLoginUseCase
    {
        Task<LoginResponseDto> LoginAsync(UserLoginDto dto);
    }
}
