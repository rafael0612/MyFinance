using Blazored.LocalStorage;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace MyFinance.Client.Services
{
    public class AuthService
    {
        private readonly ILocalStorageService _localStorage;

        public AuthService(ILocalStorageService localStorage)
        {
            _localStorage = localStorage;
        }

        public async Task<Guid?> GetUserIdAsync()
        {
            var token = await _localStorage.GetItemAsync<string>("authToken");
            if (string.IsNullOrWhiteSpace(token))
                return null;

            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(token);

            var userIdClaim = jwt.Claims.FirstOrDefault(c => c.Type == "userId");
            if (userIdClaim != null && Guid.TryParse(userIdClaim.Value, out var userId))
                return userId;

            return null;
        }

        // Puedes agregar otros métodos para obtener más claims si lo necesitas
        public async Task<string?> GetEmailAsync()
        {
            var token = await _localStorage.GetItemAsync<string>("authToken");
            if (string.IsNullOrWhiteSpace(token))
                return null;

            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(token);

            // El email suele estar en el claim "sub" o "email"
            var emailClaim = jwt.Claims.FirstOrDefault(c => c.Type == "email" || c.Type == ClaimTypes.Email || c.Type == JwtRegisteredClaimNames.Sub);
            return emailClaim?.Value;
        }

        public async Task<string?> GetFullNameAsync()
        {
            var token = await _localStorage.GetItemAsync<string>("authToken");
            if (string.IsNullOrWhiteSpace(token))
                return null;

            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(token);

            // El nombre completo debe estar incluido como claim "fullName" en el JWT
            var nameClaim = jwt.Claims.FirstOrDefault(c => c.Type == "fullName");
            return nameClaim?.Value;
        }
        public async Task<string?> GetUserTypeAsync()
        {
            var token = await _localStorage.GetItemAsync<string>("authToken");
            if (string.IsNullOrWhiteSpace(token))
                return null;

            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(token);

            var userTypeClaim = jwt.Claims.FirstOrDefault(c => c.Type == "userType");
            return userTypeClaim?.Value;
        }
    }
}