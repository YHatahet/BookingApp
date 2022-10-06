using Microsoft.AspNetCore.Mvc;
using BookingApp.Models;

using MongoDB.Bson;
using MongoDB.Driver;
using BookingApp.Services;
namespace BookingApp.Controllers;

[ApiController, Route("rooms")]
public class RoomController : ControllerBase
{

    private readonly IMongoCollection<Room> _roomCollection;
    private readonly IMongoCollection<Hotel> _hotelCollection;

    public RoomController(BookingAppService bookingAppService)
    {
        _roomCollection = bookingAppService._roomCollection;
        _hotelCollection = bookingAppService._hotelCollection;
    }


    [HttpPost("create/{hotelid}")]
    public async Task<IActionResult> CreateRoom(string hotelid, [FromBody] Room newRoom)
    {
        var hotel = await _hotelCollection.Find<Hotel>(o => o._id == hotelid).FirstOrDefaultAsync();
        if (hotel == null) return NotFound();
        newRoom._hotel = hotelid;

        await _roomCollection.InsertOneAsync(newRoom);
        return CreatedAtAction(nameof(CreateRoom), new { id = newRoom._id }, newRoom);
    }


    [HttpGet("all/{page}/{limit}")]
    public async Task<List<Room>> GetAllRooms(int page, int limit)
    {
        return await _roomCollection.Find(new BsonDocument()).Skip(page * limit).Limit(limit).ToListAsync();
    }


    [HttpGet("{id}")]
    public async Task<Room> GetRoom(string id)
    {
        return await _roomCollection.Find(o => o._id == id).FirstOrDefaultAsync();
    }


    [HttpGet("{hotelid}")]
    public async Task<List<Room>> GetRoomsInHotel(string hotelid)
    {
        return await _roomCollection.Find(o => o._hotel == hotelid).ToListAsync();
    }

    //TODO
    // [HttpGet("{hotelid}")]
    // public async Task<Room> GetRoomsInHotel(string hotelid)
    // {}

    //TODO
    // [HttpPost("{book/{roomid}}")]
    // public async Task<Room> BookRoom(string roomid)
    // {}


    [HttpPut("{id}")]
    public async Task<Room> UpdateRoom(string id, [FromBody] Room hotelBody)
    {
        var foundRoom = await _roomCollection.Find(o => o._id == id).FirstOrDefaultAsync();
        if (hotelBody.title != null) foundRoom.title = hotelBody.title;
        if (hotelBody._hotel != null) foundRoom._hotel = hotelBody._hotel;
        if (hotelBody.description != null) foundRoom.description = hotelBody.description;
        if (hotelBody.maxTenants != null) foundRoom.maxTenants = hotelBody.maxTenants;
        if (hotelBody.pricePerNight != null) foundRoom.pricePerNight = hotelBody.pricePerNight;
        //TODO occupied dates
        await _roomCollection.ReplaceOneAsync(o => o._id == id, foundRoom);
        return foundRoom;
    }


    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteRoom(string id)
    {
        FilterDefinition<Room> filter = Builders<Room>.Filter.Eq("_id", id);
        await _roomCollection.DeleteOneAsync(filter);
        return NoContent();
    }
}
