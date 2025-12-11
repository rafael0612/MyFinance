using MyFinance.Application.UseCases;
using MyFinance.Domain.Entities;
using MyFinance.Domain.Interfaces;
using MyFinance.Shared.DTOs;

namespace MyFinance.Application.Services
{
    public class UserRegisterService : IUserRegisterUseCase
    {
        private readonly IUserRepository _repo;
        public UserRegisterService(IUserRepository repo)
        {
            _repo = repo;
        }
        public async Task<(bool Success, string Message)> RegisterAsync(UserRegisterDto dto)
        {
            // Validaciones
            if (string.IsNullOrWhiteSpace(dto.Email) || string.IsNullOrWhiteSpace(dto.Password))
                return (false, "Correo y contraseña son obligatorios.");

            if (dto.Password != dto.ConfirmPassword)
                return (false, "Las contraseñas no coinciden.");

            if (dto.Password.Length < 8)
                return (false, "La contraseña debe tener al menos 8 caracteres.");

            var existing = await _repo.GetByEmailAsync(dto.Email);
            if (existing != null)
                return (false, "El correo ya está registrado.");

            // Hash de la contraseña (usa un método seguro en producción)
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);

            var userType = UserType.Standard; // Por defecto
            // permitir admins, puedes agregar lógica 

            var user = new User(dto.Email, passwordHash, userType);
            await _repo.AddAsync(user);

            return (true, "Registro exitoso.");
        }
    }
}
