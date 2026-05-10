namespace HouseHunter.Models;

public class HouseListing
{
    public string Address { get; set; } = string.Empty;

    public string City { get; set; } = string.Empty;

    public decimal Price { get; set; }

    public int Bedrooms { get; set; }

    public decimal Bathrooms { get; set; }

    public int SquareFeet { get; set; }
}
