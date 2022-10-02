namespace BookingApp.Models;

public class BookingAppDBSettings
{
    public string ConnectionString { get; set; } = String.Empty;

    public string DatabaseName { get; set; } = String.Empty;

    public string HotelCollectionName { get; set; } = String.Empty;
    public string RoomCollectionName { get; set; } = String.Empty;
    public string UserCollectionName { get; set; } = String.Empty;
    public string ReviewCollectionName { get; set; } = String.Empty;
}
