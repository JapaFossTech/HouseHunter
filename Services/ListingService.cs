using HouseHunter.Models;
using Microsoft.Extensions.DependencyInjection;

namespace HouseHunter.Services;

public class ListingService : IListingService
{
    private readonly IListingApiClient _listingApiClient;

    public ListingService(IServiceProvider serviceProvider)
    {
        _listingApiClient = serviceProvider.GetRequiredService<IListingApiClient>();
    }

    public async Task<List<HouseListing>> GetListingsAsync(ListingFilter filter)
    {
        var listings = await _listingApiClient.GetListingsAsync();

        var filteredListings = listings
            .Select(listing =>
            {
                listing.Score = CalculateScore(listing);
                return listing;
            })
            .Where(listing => filter.MinPrice == null || listing.Price >= filter.MinPrice)
            .Where(listing => filter.MaxPrice == null || listing.Price <= filter.MaxPrice)
            .Where(listing => filter.MinBedrooms == null 
                    || listing.Bedrooms >= filter.MinBedrooms)
            .Where(listing => filter.MaxPricePerSquareFoot == null 
                    || listing.PricePerSquareFoot <= filter.MaxPricePerSquareFoot)
            .OrderByDescending(listing => listing.Score)
            .ToList();

        return filteredListings;
    }

    private static decimal CalculateScore(HouseListing listing)
    {
        var bedroomPoints = listing.Bedrooms * 25m;
        var pricePerSquareFootPenalty = listing.PricePerSquareFoot / 10m;
        var pricePenalty = listing.Price / 100000m;

        return bedroomPoints - pricePerSquareFootPenalty - pricePenalty;
    }
}
