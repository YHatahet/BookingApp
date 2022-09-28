using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;


namespace BookingApp.Models;

public class Room
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string _id { get; set; }
    public string title { get; set; } //TODO make unique
    [BsonRepresentation(BsonType.ObjectId)]
    public string _hotel { get; set; }
    public string description { get; set; }
    public int maxTenants { get; set; }
    public int pricePerNight { get; set; }
    public string[]? facilites { get; set; } = null; //TODO
    // public [{int number, int occupiedDates}] rooms { get; set; } //TODO
}