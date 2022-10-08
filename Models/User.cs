using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;


namespace BookingApp.Models;

[BsonIgnoreExtraElements]
public class User
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? _id { get; set; }
    public string? username { get; set; }
    public string? email { get; set; }
    public string? password { get; set; }
    public string? role { get; set; } = "user"; //user or admin. Defaults to user role
}