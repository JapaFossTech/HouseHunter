namespace HouseHunter.Models;

public class ListingFilter
{
    public decimal? MinPrice { get; set; }

    public decimal? MaxPrice { get; set; }

    public int? MinBedrooms { get; set; }

    public decimal? MaxPricePerSquareFoot { get; set; }
}
