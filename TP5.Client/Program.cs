using Blazored.SessionStorage;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using TP5.Client;
using TP5.Client.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

// session du navigateur
builder.Services.AddBlazoredSessionStorage();

// un client par API
builder.Services.AddHttpClient<IAuthApiService, AuthApiService>(client =>
{
    client.BaseAddress = new Uri("http://localhost:5299/");
});


builder.Services.AddHttpClient<IOrdinateurApiService, OrdinateurApiService>(client =>
{
    client.BaseAddress = new Uri("http://localhost:5010/");
});


builder.Services.AddHttpClient<IReparationApiService, ReparationApiService>(client =>
{
    client.BaseAddress = new Uri("http://localhost:5010/");
});



await builder.Build().RunAsync();