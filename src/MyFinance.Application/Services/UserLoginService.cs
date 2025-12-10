using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MyFinance.Application.UseCases;
using MyFinance.Domain.Interfaces;
using MyFinance.Shared.DTOs;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MyFinance.Application.Services
{
    public class UserLoginService : IUserLoginUseCase
    {
        private readonly IUserRepository _repo;
        private readonly IConfiguration _config;
        public UserLoginService(IUserRepository repo, IConfiguration config)
        {
            _repo = repo;
            _config = config;
        }

        public async Task<LoginResponseDto> LoginAsync(UserLoginDto dto)
        {
            var user = await _repo.GetByEmailAsync(dto.Email);
            if (user == null || !user.IsActive || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
            {
                return new LoginResponseDto { Message = "Credenciales incorrectas." };
            }

            // Generar JWT
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim("userId", user.Id.ToString()),
                new Claim("fullName", user.FullName!)
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(8),
                signingCredentials: creds
            );

            return new LoginResponseDto
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Message = "Login exitoso."
            };
        }
    }
}
