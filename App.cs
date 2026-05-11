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

        var listings = await _listingService.GetListingsAsync();

        Console.WriteLine();
        Console.WriteLine("Available Listings");
        Console.WriteLine("------------------");

        foreach (var listing in listings)
        {
            DisplayListing(listing);
        }
    }

    private static void DisplayListing(HouseListing listing)
    {
        Console.WriteLine();
        Console.WriteLine($"{listing.Address}");
        Console.WriteLine($"{listing.City}");
        Console.WriteLine($"Price: {listing.Price:C}");
        Console.WriteLine($"Size: {listing.SquareFeet:N0} sq ft");
        Console.WriteLine($"Price per sq ft: {listing.PricePerSquareFoot:C}");
        Console.WriteLine($"Beds/Baths: {listing.Bedrooms} bed / {listing.Bathrooms} bath");
    }
}
