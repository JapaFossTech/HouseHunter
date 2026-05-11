using HouseHunter.Models;
using HouseHunter.Services;
using Microsoft.Extensions.DependencyInjection;

public class App
{
    private readonly IListingService _listingService;

    public App(IServiceProvider serviceProvider)
    {
        _listingService = serviceProvider.GetRequiredService<IListingService>();
    }

    public async Task RunAsync()
    {
        Console.WriteLine("HouseHunter started.");

        var filter = new ListingFilter
        {
            MinPrice = 300000m,
            MaxPrice = 450000m,
            MinBedrooms = 4,
            MaxPricePerSquareFoot = 170m
        };

        DisplayFilter(filter);

        var listings = await _listingService.GetListingsAsync(filter);

        Console.WriteLine();
        Console.WriteLine("Available Listings");
        Console.WriteLine("------------------");

        foreach (var listing in listings)
        {
            DisplayListing(listing);
        }
    }

    private static void DisplayFilter(ListingFilter filter)
    {
        Console.WriteLine();
        Console.WriteLine("Active Filters");
        Console.WriteLine("--------------");
        Console.WriteLine($"Minimum price: {DisplayCurrency(filter.MinPrice)}");
        Console.WriteLine($"Maximum price: {DisplayCurrency(filter.MaxPrice)}");
        Console.WriteLine($"Minimum bedrooms: {DisplayNumber(filter.MinBedrooms)}");
        Console.WriteLine($"Maximum price per sq ft: {DisplayCurrency(filter.MaxPricePerSquareFoot)}");
    }

    private static void DisplayListing(HouseListing listing)
    {
        Console.WriteLine();
        Console.WriteLine($"{listing.Address}");
        Console.WriteLine($"{listing.City}");
        Console.WriteLine($"Price: {listing.Price:C0}");
        Console.WriteLine($"Size: {listing.SquareFeet:N0} sq ft");
        Console.WriteLine($"Price per sq ft: {listing.PricePerSquareFoot:C0}");
        Console.WriteLine($"Beds/Baths: {listing.Bedrooms} bed / {listing.Bathrooms} bath");
        Console.WriteLine($"Score: {listing.Score:N1}");
    }

    private static string DisplayCurrency(decimal? value)
    {
        return value == null ? "Any" : value.Value.ToString("C0");
    }

    private static string DisplayNumber(int? value)
    {
        return value == null ? "Any" : value.Value.ToString();
    }
}
