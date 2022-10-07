using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
namespace BookingApp.Models;

public class ReservationType
{
    public DateTime start;
    public DateTime end;
}

public class RoomEntry
{
    public int? roomNumber;
    public List<ReservationType>? occupiedDates;
}


public class Room
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? _id { get; set; }
    public string? title { get; set; }
    [BsonRepresentation(BsonType.ObjectId)]
    public string? _hotel { get; set; }
    public string? description { get; set; }
    public int? maxTenants { get; set; }
    public double? pricePerNight { get; set; }
    public string[]? facilities { get; set; } = null;
    public List<RoomEntry>? rooms { get; set; }
}


public class BookRoomBody
{

    public int? roomNumber { get; set; }
    public string? startDate { get; set; }
    public string? endDate { get; set; }
    public int? numOfTenants { get; set; }

}