using HouseHunter.Models;

namespace HouseHunter.Services;

public class ListingService : IListingService
{
    public Task<List<HouseListing>> GetListingsAsync(ListingFilter filter)
    {
        var listings = new List<HouseListing>
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

        var filteredListings = listings
            .Select(listing =>
            {
                listing.Score = CalculateScore(listing);
                return listing;
            })
            .Where(listing => filter.MinPrice == null || listing.Price >= filter.MinPrice)
            .Where(listing => filter.MaxPrice == null || listing.Price <= filter.MaxPrice)
            .Where(listing => filter.MinBedrooms == null || listing.Bedrooms >= filter.MinBedrooms)
            .Where(listing => filter.MaxPricePerSquareFoot == null || listing.PricePerSquareFoot <= filter.MaxPricePerSquareFoot)
            .OrderByDescending(listing => listing.Score)
            .ToList();

        return Task.FromResult(filteredListings);
    }

    private static decimal CalculateScore(HouseListing listing)
    {
        var bedroomPoints = listing.Bedrooms * 25m;
        var pricePerSquareFootPenalty = listing.PricePerSquareFoot / 10m;
        var pricePenalty = listing.Price / 100000m;

        return bedroomPoints - pricePerSquareFootPenalty - pricePenalty;
    }
}
