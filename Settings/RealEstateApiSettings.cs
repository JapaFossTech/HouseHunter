namespace HouseHunter.Settings;

public class RealEstateApiSettings
{
    public string BaseUrl { get; set; } = string.Empty;

    public string ApiKey { get; set; } = string.Empty;

    public bool EnableLiveApiCalls { get; set; }
}
