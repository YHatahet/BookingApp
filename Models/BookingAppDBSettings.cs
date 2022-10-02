namespace BookingAppApi.Models;

public class BookingAppDBSettings
{
    public string ConnectionString { get; set; } = String.Empty;

    public string DatabaseName { get; set; } = String.Empty;

    public string HotelCollectionName { get; set; } = String.Empty;
    public string RoomsCollectionName { get; set; } = String.Empty;
    public string UsersCollectionName { get; set; } = String.Empty;
    public string ReviewsCollectionName { get; set; } = String.Empty;
}
