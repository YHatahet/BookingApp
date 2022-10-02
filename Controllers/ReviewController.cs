// using Microsoft.Extensions.Configuration;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using BookingApp.Models;

using MongoDB.Bson;
using MongoDB.Driver;

namespace BookingApp.Controllers;

[ApiController, Route("reviews")]
public class ReviewController  : ControllerBase
{

    private IMongoCollection<Hotel> _hotelCollection;
    public ReviewController(IMongoClient client)
    {
        var db = client.GetDatabase("hotelCollection");
        _hotelCollection = db.GetCollection<Hotel>("hotels");
    }

    [HttpGet]
    public IEnumerable<Hotel> Get()
    {
        return _hotelCollection.Find(_ => true).ToList();
    }
}
