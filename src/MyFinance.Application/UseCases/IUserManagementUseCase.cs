using MyFinance.Shared.DTOs;

namespace MyFinance.Application.UseCases
{
    public interface IUserManagementUseCase
    {
        Task<IEnumerable<UserDto>> GetAllAsync();
        Task<UserDto?> GetByIdAsync(Guid id);
        Task<bool> ActivateAsync(Guid id);
        Task<bool> DeactivateAsync(Guid id);
        Task<bool> DeleteAsync(Guid id);
        Task<bool> UpdateAsync(UserDto dto);
        Task<(bool Success, string Message)> CreateAsync(UserRegisterDto dto);
    }
}
