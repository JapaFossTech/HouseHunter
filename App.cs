using HouseHunter.Services;

public class App
{
    private readonly IListingService _listingService;

    public App(IListingService listingService)
    {
        _listingService = listingService;
    }

    public async Task RunAsync()
    {
        Console.WriteLine("HouseHunter started.");

        var listings = await _listingService.GetListingsAsync();

        foreach (var listing in listings)
        {
            Console.WriteLine();
            Console.WriteLine($"{listing.Address}, {listing.City}");
            Console.WriteLine($"Price: {listing.Price:C}");
            Console.WriteLine($"Bedrooms: {listing.Bedrooms}");
            Console.WriteLine($"Bathrooms: {listing.Bathrooms}");
            Console.WriteLine($"Square Feet: {listing.SquareFeet}");
        }
    }
}
