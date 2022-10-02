using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;


namespace BookingApp.Models;
[BsonIgnoreExtraElements]
public class Hotel
{
    [BsonId] //primary key
    [Required, BsonRepresentation(BsonType.ObjectId)]
    public string _id { get; set; }
    [Required, BsonRepresentation(BsonType.ObjectId)]
    public string _owner { get; set; }
    [Required]
    public string name { get; set; } //TODO make unique
    [Required]
    public string description { get; set; }
    [Required]
    public string address { get; set; }
    [Required]
    public string city { get; set; }
    [Required]
    public double distanceFromCenter { get; set; }
    public double? rating { get; set; } = 0;
    public int? numOfRatings { get; set; } = 0;
    public bool? featured { get; set; } = false;
}