using HouseHunter.Services;
using HouseHunter.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder(args);

builder.Configuration.AddJsonFile("appsetting.Development.json", optional: true, reloadOnChange: true);

builder.Services.Configure<RealEstateApiSettings>(
    builder.Configuration.GetSection("RealEstateApi"));

builder.Services.AddTransient<App>();
builder.Services.AddTransient<IListingService, ListingService>();
builder.Services.AddHttpClient("RealEstateApi");
builder.Services.AddTransient<IListingApiClient, ListingApiClient>();

using var host = builder.Build();

var app = host.Services.GetRequiredService<App>();

await app.RunAsync();
