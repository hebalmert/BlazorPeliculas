using BlazorPeliculas.Client;
using BlazorPeliculas.Client.Repositorios;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

//Se ejecuta los servicios que tenemos Centralizados abajo
ConfigureServices(builder.Services);

await builder.Build().RunAsync();


//La manera organizada es Centralizar la declaracion de Servicios
void ConfigureServices(IServiceCollection services)
{
    services.AddSingleton<IRepositorio, Repositorio>();
}
