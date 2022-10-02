using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;


namespace BookingApp.Models;

public class User
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? _id { get; set; }
    public string username { get; set; } //TODO make unique
    public string email { get; set; } //TODO make unique
    public string password { get; set; }
    private bool? isAdmin { get; set; } = false;
}