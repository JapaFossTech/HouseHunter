using HouseHunter.Models;
using HouseHunter.Settings;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Net.Http;
using System.Reflection.Emit;
using System.Text;
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
        if (!_settings.EnableLiveApiCalls)
        {
            await PrintNewestApiDumpAsync();
            return GetFallbackListings();
        }

        var httpClient = _httpClientFactory.CreateClient("RealEstateApi");

        if (Uri.TryCreate(_settings.BaseUrl, UriKind.Absolute, out var baseUrl))
        {
            httpClient.BaseAddress = baseUrl;
        }

        if (!string.IsNullOrWhiteSpace(_settings.ApiKey))
        {
            httpClient.DefaultRequestHeaders.Add("X-API-Key", _settings.ApiKey);
        }

        try
        {
            var requestPayload = new
            {
                search = "77375",
                //zipcodes = new[] {77375, 77389},
                type = "sale",
                property_type = "multi_family", //new[] { "house", "multi_family" },
                price_min = 370000,
                price_max = 470000,
                max_items = 40
            };

            var json = JsonSerializer.Serialize(requestPayload);
            using var content = new StringContent(json, Encoding.UTF8, "application/json");
            using var response = await httpClient.PostAsync("/v1/properties", content);
            var responseBody = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Raw API response:");
                Console.WriteLine(responseBody);

                await SaveApiDumpAsync(responseBody);
            }
            else
            {
                Console.WriteLine($"API request failed with status code: {(int)response.StatusCode} {response.StatusCode}");
                Console.WriteLine("Response body:");
                Console.WriteLine(responseBody);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"API request failed: {ex.Message}");
        }

        return GetFallbackListings();
    }

    private static async Task PrintNewestApiDumpAsync()
    {
        var dumpFolder = GetDumpFolder();

        if (!Directory.Exists(dumpFolder))
        {
            return;
        }

        var newestDump = Directory
            .GetFiles(dumpFolder, "*.json")
            .OrderByDescending(File.GetLastWriteTimeUtc)
            .FirstOrDefault();

        if (newestDump == null)
        {
            return;
        }

        var json = await File.ReadAllTextAsync(newestDump);

        Console.WriteLine("Loaded newest API dump:");
        Console.WriteLine(json);
    }

    private static async Task SaveApiDumpAsync(string responseBody)
    {
        try
        {
            Directory.CreateDirectory(GetDumpFolder());

            using var jsonDocument = JsonDocument.Parse(responseBody);
            var prettyJson = JsonSerializer.Serialize(jsonDocument.RootElement, new JsonSerializerOptions
            {
                WriteIndented = true
            });

            var fileName = $"api-response-{DateTimeOffset.Now:yyyyMMdd-HHmmss}.json";
            var filePath = Path.Combine(GetDumpFolder(), fileName);

            await File.WriteAllTextAsync(filePath, prettyJson);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Could not save API dump: {ex.Message}");
        }
    }

    private static string GetDumpFolder()
    {
        return Path.Combine(Directory.GetCurrentDirectory(), "ApiDumps");
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
