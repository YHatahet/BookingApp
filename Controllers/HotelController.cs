// using Microsoft.Extensions.Configuration;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using BookingApp.Models;

using MongoDB.Bson;
using MongoDB.Driver;

namespace BookingApp.Controllers;

[ApiController, Route("hotels")]
public class HotelController : ControllerBase
{

    private IMongoCollection<Hotel> _hotelCollection;
    public HotelController(IMongoClient client)
    {
        var db = client.GetDatabase("hotelCollection");
        _hotelCollection = db.GetCollection<Hotel>("hotels");
    }

    [HttpGet, Route("all/{page}/{limit}")]
    public IEnumerable<Hotel> GetAllHotels(int page, int limit)
    {
        return _hotelCollection.Find(_ => true).Skip(page * limit).Limit(limit).ToList();
    }

    [HttpGet, Route("{id}")]
    public Hotel GetHotelFromId(string id)
    {
        return _hotelCollection.Find(o => o._id == id).ToList()[0];
    }

    [HttpGet, Route("topRated/{amount}")]
    public IEnumerable<Hotel> GetTopRatedHotels(int amount)
    {
        return _hotelCollection.Find(o => o.rating != null).SortByDescending(o => o.rating).Limit(amount).ToList();
    }
}
