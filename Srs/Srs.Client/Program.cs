using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Blazored.Modal;
using Blazored.Toast;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

// Debug: Print the base address
Console.WriteLine($"HostEnvironment.BaseAddress: {builder.HostEnvironment.BaseAddress}");

builder.Services.AddScoped(sp =>
{
    var httpClient = new HttpClient
    {
        BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)
    };
    Console.WriteLine($"HttpClient BaseAddress configured: {httpClient.BaseAddress}");
    return httpClient;
});

// Add Blazored Modal
builder.Services.AddBlazoredModal();

// Add Blazored Toast
builder.Services.AddBlazoredToast();

await builder.Build().RunAsync();