using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;


namespace BookingAppApi.Models;

public class Hotel
{
    [BsonId] //primary key
    [BsonRepresentation(BsonType.ObjectId)]

    public string name { get; set; }
    public string _owner { get; set; } //TODO
    public int rating { get; set; }
    public int numOfRatings { get; set; }
    public string description { get; set; }
    public string address { get; set; }
    public string city { get; set; }
    public int distanceFromCenter { get; set; }
    public bool featured { get; set; }
}