using HouseHunter.Models;

namespace HouseHunter.Services;

public interface IListingService
{
    Task<List<HouseListing>> GetListingsAsync(ListingFilter filter);
}
