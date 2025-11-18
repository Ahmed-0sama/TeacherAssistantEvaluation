using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

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

await builder.Build().RunAsync();