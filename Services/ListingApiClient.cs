using HouseHunter.Models;
using HouseHunter.Settings;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Net.Http;
using System.Text.Json;

namespace HouseHunter.Services;

public class ListingApiClient : IListingApiClient
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly RealEstateApiSettings _settings;

    public ListingApiClient(IServiceProvider serviceProvider)
    {
        _httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();
        _settings = serviceProvider
            .GetRequiredService<IOptions<RealEstateApiSettings>>()
            .Value;
    }

    public async Task<List<HouseListing>> GetListingsAsync()
    {
        var httpClient = _httpClientFactory.CreateClient("RealEstateApi");

        if (Uri.TryCreate(_settings.BaseUrl, UriKind.Absolute, out var baseUrl))
        {
            httpClient.BaseAddress = baseUrl;
        }

        if (!string.IsNullOrWhiteSpace(_settings.ApiKey))
        {
            httpClient.DefaultRequestHeaders.Add("X-Api-Key", _settings.ApiKey);
        }

        await Task.Delay(500);

        try
        {
            var filePath = Path.Combine(AppContext.BaseDirectory, "listings.json");
            var json = await File.ReadAllTextAsync(filePath);
            var listings = JsonSerializer
                                .Deserialize<List<HouseListing>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return listings ?? GetFallbackListings();
        }
        catch
        {
            return GetFallbackListings();
        }
    }

    private static List<HouseListing> GetFallbackListings()
    {
        return new List<HouseListing>
        {
            new()
            {
                Address = "123 Maple Street",
                City = "Springfield",
                Price = 285000m,
                Bedrooms = 3,
                Bathrooms = 2,
                SquareFeet = 1650
            },
            new()
            {
                Address = "456 Oak Avenue",
                City = "Riverton",
                Price = 342500m,
                Bedrooms = 4,
                Bathrooms = 2.5m,
                SquareFeet = 2150
            },
            new()
            {
                Address = "789 Pine Lane",
                City = "Lakeside",
                Price = 415000m,
                Bedrooms = 5,
                Bathrooms = 3,
                SquareFeet = 2800
            }
        };
    }
}
