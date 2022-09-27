using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;


namespace BookingApp.Models;
[BsonIgnoreExtraElements]
public class Hotel
{
    [BsonId] //primary key
    [BsonRepresentation(BsonType.ObjectId)]
    public string _id { get; set; }
    [BsonRepresentation(BsonType.ObjectId)]
    public string _owner { get; set; }
    public string name { get; set; }
    public string description { get; set; }
    public string address { get; set; }
    public string city { get; set; }
    public double distanceFromCenter { get; set; }
    public double? rating { get; set; } = 0;
    public int? numOfRatings { get; set; } = 0;
    public bool? featured { get; set; } = false;
}