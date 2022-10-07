using Microsoft.AspNetCore.Mvc;
using BookingApp.Models;

using MongoDB.Bson;
using MongoDB.Driver;
using BookingApp.Services;
namespace BookingApp.Controllers;

[ApiController, Route("hotels")]
public class HotelController : ControllerBase
{

    private readonly IMongoCollection<Hotel> _hotelCollection;

    public HotelController(BookingAppService bookingAppService)
    {
        _hotelCollection = bookingAppService._hotelCollection;
    }


    [HttpPost("create")]
    public async Task<IActionResult> CreateHotel([FromBody] Hotel newHotel)
    {
        await _hotelCollection.InsertOneAsync(newHotel);
        return CreatedAtAction(nameof(CreateHotel), new { id = newHotel._id }, newHotel);

    }


    [HttpGet, Route("all/{page}/{limit}")]
    public async Task<List<Hotel>> GetAllHotels(int page, int limit)
    {
        return await _hotelCollection.Find(new BsonDocument()).Skip(page * limit).Limit(limit).ToListAsync();
    }


    [HttpGet("{{id}}")]
    public async Task<Hotel> GetHotel(string id)
    {
        return await _hotelCollection.Find(o => o._id == id).FirstOrDefaultAsync();
    }

    [HttpGet, Route("topRated/{amount}")]
    public Task<List<Hotel>> GetTopRatedHotels(int amount)
    {
        return _hotelCollection.Find(o => o.rating != null).SortByDescending(o => o.rating).Limit(amount).ToListAsync();
    }


    [HttpPut("{{id}}")]
    public async Task<Hotel> UpdateHotel(string id, [FromBody] Hotel hotelBody)
    {
        var foundHotel = await _hotelCollection.Find(o => o._id == id).FirstOrDefaultAsync();
        if (hotelBody.name != null) foundHotel.name = hotelBody.name;
        if (hotelBody._owner != null) foundHotel._owner = hotelBody._owner;
        if (hotelBody.description != null) foundHotel.description = hotelBody.description;
        if (hotelBody.address != null) foundHotel.address = hotelBody.address;
        if (hotelBody.city != null) foundHotel.city = hotelBody.city;
        if (hotelBody.distanceFromCenter != null) foundHotel.distanceFromCenter = hotelBody.distanceFromCenter;
        if (hotelBody.rating != null) foundHotel.rating = hotelBody.rating;
        if (hotelBody.numOfRatings != null) foundHotel.numOfRatings = hotelBody.numOfRatings;
        if (hotelBody.featured != null) foundHotel.featured = hotelBody.featured;
        await _hotelCollection.ReplaceOneAsync(o => o._id == id, foundHotel);
        return foundHotel;
    }


    [HttpDelete("{{id}}")]
    public async Task<IActionResult> DeleteHotel(string id)
    {
        FilterDefinition<Hotel> filter = Builders<Hotel>.Filter.Eq("_id", id);
        await _hotelCollection.DeleteOneAsync(filter);
        return NoContent();
    }
}
