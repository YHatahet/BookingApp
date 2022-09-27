using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;


namespace BookingAppApi.Models;

public class Review
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string _id { get; set; }
    public int rating { get; set; }
    public string review { get; set; }
    public string _hotel { get; set; } //TODO
    public string _user { get; set; } //TODO
}