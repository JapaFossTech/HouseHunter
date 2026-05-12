using HouseHunter.Models;

namespace HouseHunter.Services;

public interface IListingApiClient
{
    Task<List<HouseListing>> GetListingsAsync();
}
