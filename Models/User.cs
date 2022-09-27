using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;


namespace BookingAppApi.Models;

public class User
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]

    public string username { get; set; }
    public string email { get; set; }
    public string password { get; set; }
    public bool isAdmin { get; set; }
}