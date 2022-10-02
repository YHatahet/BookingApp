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


    [HttpGet("{id}")]
    public async Task<Hotel> GetHotel(string id)
    {
        return await _hotelCollection.Find(o => o._id == id).FirstOrDefaultAsync();
    }

    [HttpGet, Route("topRated/{amount}")]
    public Task<List<Hotel>> GetTopRatedHotels(int amount)
    {
        return _hotelCollection.Find(o => o.rating != null).SortByDescending(o => o.rating).Limit(amount).ToListAsync();
    }


    // [HttpPut("{id}")]
    // public async Task<IActionResult> UpdateHotel(string id, [FromBody] Hotel hotelBody)
    // {
    //     FilterDefinition<Hotel> filter = Builders<Hotel>.Filter.Eq("_id", id);
    //     // UpdateDefinition<Hotel> update = Builders<Hotel>.Update.Combine(hotelBody.to);
    //     UpdateDefinition<Hotel> update = Builders<Hotel>.Update.Combine();
    //     // await _hotelCollection.UpdateOneAsync(filter, update);
    //     return NoContent();

    // }


    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteHotel(string id)
    {
        FilterDefinition<Hotel> filter = Builders<Hotel>.Filter.Eq("_id", id);
        await _hotelCollection.DeleteOneAsync(filter);
        return NoContent();
    }
}
