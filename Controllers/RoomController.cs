// using Microsoft.Extensions.Configuration;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using BookingApp.Models;

using MongoDB.Bson;
using MongoDB.Driver;

namespace BookingApp.Controllers;

[ApiController, Route("rooms")]
public class RoomController  : ControllerBase
{

    private IMongoCollection<Hotel> _hotelCollection;
    public RoomController(IMongoClient client)
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
