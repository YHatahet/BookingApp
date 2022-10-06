using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public struct ReservationType
{
    public DateTime start;
    public DateTime end;
}


public struct RoomType
{
    public int? roomNumber;
    public ReservationType[]? occupiedDates;
}

namespace BookingApp.Models
{
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
        public string[]? facilites { get; set; } = null;
        public RoomType[] rooms { get; set; }
    }
}