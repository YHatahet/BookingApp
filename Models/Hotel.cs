using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;


namespace BookingApp.Models;
[BsonIgnoreExtraElements]
public class Hotel
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? _id { get; set; }
    [BsonRepresentation(BsonType.ObjectId)]
    public string? _owner { get; set; }
    public string? name { get; set; } //TODO make unique
    public string? description { get; set; }
    public string? address { get; set; }
    public string? city { get; set; }
    public double? distanceFromCenter { get; set; }
    public double? rating { get; set; }
    public int? numOfRatings { get; set; }
    public bool? featured { get; set; }
}