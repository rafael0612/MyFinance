using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection; // <-- para AddHttpClient / IHttpClientFactory
using MudBlazor.Services;
using Myfinance.Client;                      // Asegúrate que el namespace coincide con tu proyecto
using MyFinance.Client.Auth;                // Donde está JwtAuthenticationStateProvider
using MyFinance.Client.Services;
using System.Net.Http;                      // (normalmente viene implícito, pero lo dejo explícito)

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// UI y almacenamiento local
builder.Services.AddMudServices();
builder.Services.AddBlazoredLocalStorage();

// Autenticación / Autorización
builder.Services.AddAuthorizationCore();

// Registramos el provider concreto...
builder.Services.AddScoped<JwtAuthenticationStateProvider>();
// ...y lo exponemos como AuthenticationStateProvider
builder.Services.AddScoped<AuthenticationStateProvider>(sp =>
    sp.GetRequiredService<JwtAuthenticationStateProvider>());

// Handler que añade el token en cada request
builder.Services.AddTransient<AuthorizationMessageHandler>();
// Servicio de autenticación personalizado
builder.Services.AddScoped<AuthService>();

// HttpClient para la API protegida
builder.Services.AddHttpClient("AuthorizedAPI", client =>
{
    client.BaseAddress = new Uri("http://localhost:5134/");  
    client.Timeout = TimeSpan.FromMinutes(5);
})
.AddHttpMessageHandler<AuthorizationMessageHandler>();

// HttpClient por defecto (el que usas con @inject HttpClient Http)
builder.Services.AddScoped(sp =>
    sp.GetRequiredService<IHttpClientFactory>().CreateClient("AuthorizedAPI"));

await builder.Build().RunAsync();
