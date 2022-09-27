using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;


namespace BookingAppApi.Models;

public class Room
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string _id { get; set; }
    public string title { get; set; }
    public string _hotel { get; set; } //TODO
    public string description { get; set; }
    public int maxTenants { get; set; }
    public int pricePerNight { get; set; }
    public string[] facilites { get; set; }
    // public [{int number, int occupiedDates}] rooms { get; set; } //TODO
}