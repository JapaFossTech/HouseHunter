using HouseHunter.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddTransient<App>();
builder.Services.AddTransient<IListingService, ListingService>();

using var host = builder.Build();

var app = host.Services.GetRequiredService<App>();

await app.RunAsync();
