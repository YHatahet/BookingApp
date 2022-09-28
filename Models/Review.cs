using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;


namespace BookingApp.Models;

public class Review
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string _id { get; set; }
    public int rating { get; set; }
    public string review { get; set; }
    [BsonRepresentation(BsonType.ObjectId)]
    public string _hotel { get; set; }
    [BsonRepresentation(BsonType.ObjectId)]
    public string _user { get; set; }
}