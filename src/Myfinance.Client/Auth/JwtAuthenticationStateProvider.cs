using Blazored.LocalStorage;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Components.Authorization;

namespace MyFinance.Client.Auth
{
    public class JwtAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly ILocalStorageService _localStorage;
        private readonly HttpClient _httpClient;

        public JwtAuthenticationStateProvider(ILocalStorageService localStorage, HttpClient httpClient)
        {
            _localStorage = localStorage;
            _httpClient = httpClient;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var token = await _localStorage.GetItemAsync<string>("authToken");
            //var identity = string.IsNullOrWhiteSpace(token)
            //    ? new ClaimsIdentity()
            //    : new ClaimsIdentity(ParseClaimsFromJwt(token), "jwt");

            if (string.IsNullOrWhiteSpace(token))
            {
                // Usuario anónimo
                var anonymous = new ClaimsPrincipal(new ClaimsIdentity());
                return new AuthenticationState(anonymous);
            }

            // Si usas AuthorizationMessageHandler, esto no es obligatorio,
            // pero no molesta:
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            var identity = new ClaimsIdentity(
                ParseClaimsFromJwt(token),
                "jwt");

            return new AuthenticationState(new ClaimsPrincipal(identity));
        }

        public void NotifyUserAuthentication(string token)
        {
            var identity = new ClaimsIdentity(ParseClaimsFromJwt(token), "jwt");
            var user = new ClaimsPrincipal(identity);
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
        }

        public void NotifyUserLogout()
        {
            var anonymous = new ClaimsPrincipal(new ClaimsIdentity());
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(anonymous)));
        }

        // Utilidad para extraer claims del JWT
        private IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
        {
            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(jwt);
            return token.Claims;
        }
    }
}