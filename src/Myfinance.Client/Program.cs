using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Myfinance.Client;
using MudBlazor.Services;

// public partial class Program
// {
//     public static async Task Main(string[] args)
//     {
        var builder = WebAssemblyHostBuilder.CreateDefault(args);
        builder.RootComponents.Add<App>("#app");
        builder.RootComponents.Add<HeadOutlet>("head::after");

        builder.Services.AddScoped(
            sp => new HttpClient 
            { 
                BaseAddress = new Uri("http://localhost:5134"),
                Timeout = TimeSpan.FromMinutes(5) // Configura un timeout de 5 minutos
            }
        );

        builder.Services.AddMudServices();

        await builder.Build().RunAsync();
//     }
// }