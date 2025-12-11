using MyFinance.Application.UseCases;
using MyFinance.Domain.Entities;
using MyFinance.Domain.Interfaces;
using MyFinance.Shared.DTOs;

namespace MyFinance.Application.Services
{
    public class UserManagementService : IUserManagementUseCase
    {
        private readonly IUserRepository _repo;
        public UserManagementService(IUserRepository repo) => _repo = repo;

        public async Task<IEnumerable<UserDto>> GetAllAsync()
            => (await _repo.GetAllAsync()).Select(u => new UserDto
            {
                Id = u.Id,
                Email = u.Email,
                FullName = u.FullName,
                IsActive = u.IsActive,
                NameUser = u.NameUser,
                LastName = u.LastName,
                UserType = (int)u.UserType,
                CreatedAt = u.CreatedAt
            });

        public async Task<UserDto?> GetByIdAsync(Guid id)
        {
            var u = await _repo.GetByIdAsync(id);
            return u == null ? null : new UserDto
            {
                Id = u.Id,
                Email = u.Email,
                FullName = u.FullName,
                IsActive = u.IsActive,
                NameUser = u.NameUser,
                LastName = u.LastName,
                UserType = (int)u.UserType,
                CreatedAt = u.CreatedAt
            };
        }

        public async Task<(bool Success, string Message)> CreateAsync(UserRegisterDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Email) || string.IsNullOrWhiteSpace(dto.Password))
                return (false, "Correo y contraseña son obligatorios.");
            if (dto.Password != dto.ConfirmPassword)
                return (false, "Las contraseñas no coinciden.");
            if (dto.Password.Length < 8)
                return (false, "La contraseña debe tener al menos 8 caracteres.");

            var existing = await _repo.GetByEmailAsync(dto.Email);
            if (existing != null)
                return (false, "El correo ya está registrado.");

            var passwordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);
            var user = new User(dto.Email, passwordHash, dto.NameUser, dto.LastName, (UserType)dto.UserType);
            await _repo.AddAsync(user);
            return (true, "Usuario creado correctamente.");
        }

        public async Task<bool> ActivateAsync(Guid id)
        {
            var user = await _repo.GetByIdAsync(id);
            if (user == null) return false;
            user.Activate();
            return await _repo.UpdateAsync(user);
        }

        public async Task<bool> DeactivateAsync(Guid id)
        {
            var user = await _repo.GetByIdAsync(id);
            if (user == null) return false;
            user.Deactivate();
            return await _repo.UpdateAsync(user);
        }

        public async Task<bool> DeleteAsync(Guid id)
            => await _repo.DeleteAsync(id);

        public async Task<bool> UpdateAsync(UserDto dto)
        {
            var user = await _repo.GetByIdAsync(dto.Id);
            if (user == null) return false;
            // Actualiza solo los campos permitidos
            user.ChangeNameUser(dto.NameUser);
            user.ChangeLastName(dto.LastName);
            user.ChangeFullName();
            user.ChangeIsActive(dto.IsActive);
            // cambiar el tipo de usuario:
            user.ChangeUserType((UserType)dto.UserType);
            return await _repo.UpdateAsync(user);
        }
    }
}
