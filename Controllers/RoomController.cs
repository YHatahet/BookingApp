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



    public struct Rooms
    {
        public int[] roomNumbers { get; set; }
    }

    [HttpPost("create/room/{roomid}")]
    public async Task<IActionResult> AddRooms(string roomid, [FromBody] Rooms rooms)
    {
        var foundRoom = await _roomCollection.Find<Room>(o => o._id == roomid).FirstOrDefaultAsync();
        if (foundRoom == null) return NotFound();
        if (foundRoom.rooms == null) foundRoom.rooms = new List<RoomEntry>();

        foreach (int roomNumber in rooms.roomNumbers)
        {
            var roomEntry = new RoomEntry();
            roomEntry.roomNumber = roomNumber;
            foundRoom.rooms.Add(roomEntry);
        }

        await _roomCollection.ReplaceOneAsync(o => o._id == roomid, foundRoom);
        return CreatedAtAction(nameof(AddRooms), new { id = foundRoom._id }, foundRoom);

    }

    [HttpGet("all/{page}/{limit}")]
    public async Task<List<Room>> GetAllRooms(int page, int limit)
    {
        return await _roomCollection.Find(new BsonDocument()).Skip(page * limit).Limit(limit).ToListAsync();
    }


    [HttpGet("{{id}}")]
    public async Task<Room> GetRoom(string id)
    {
        return await _roomCollection.Find(o => o._id == id).FirstOrDefaultAsync();
    }


    [HttpGet("{{hotelid}}")]
    public async Task<List<Room>> GetRoomsInHotel(string hotelid)
    {
        return await _roomCollection.Find(o => o._hotel == hotelid).ToListAsync();
    }


    [HttpPost("book/{roomid}")]
    public async Task<IActionResult> BookRoom(string roomid, [FromBody] BookRoomBody roomBody)
    {
        Room roomToBook = await _roomCollection.Find(o => o._id == roomid).FirstOrDefaultAsync();
        if (roomToBook == null) return NotFound("No rooms found with specified room ID");
        if (roomBody.numOfTenants > roomToBook.maxTenants) return BadRequest("Tenants exceed max allowable number of tenants");
        if (roomBody.startDate == null || roomBody.endDate == null) return BadRequest("End date cannot be greater than start date");

        string dateFormat = "yyyy-MM-dd";
        DateTime startDate = DateTime.ParseExact(roomBody.startDate, dateFormat, null);
        DateTime endDate = DateTime.ParseExact(roomBody.endDate, dateFormat, null);

        if (startDate > endDate) return BadRequest("Start Date cannot be greater than the End Date");
        if (roomToBook.rooms == null || roomToBook.rooms.Count == 0) return BadRequest("No rooms ");

        // iterate over rooms and see if any thing prevents booking
        foreach (RoomEntry room in roomToBook.rooms)
        {
            if (room.roomNumber != roomBody.roomNumber) continue; // next room
            if (room.occupiedDates == null) room.occupiedDates = new List<ReservationType>();

            // Check for time conflicts
            foreach (ReservationType reservation in room.occupiedDates)
            {
                DateTime startOccDateVal = reservation.start;
                DateTime endOccDateVal = reservation.end;

                bool isConflicting =
                  (startDate <= reservation.start && endDate >= reservation.start) ||
                  (startDate <= reservation.end && endDate >= reservation.end);

                if (!isConflicting) continue;

                return BadRequest("No vacancies for this room at the given time");
            }

            var toAdd = new ReservationType();
            toAdd.start = startDate;
            toAdd.end = endDate;

            // if no time conflicts found
            room.occupiedDates.Add(toAdd);

            await _roomCollection.ReplaceOneAsync(o => o._id == roomid, roomToBook);
            return Ok("Successfully booked");

        }
        return NotFound("No room number found with the selected room type");

    }


    [HttpPut("{{id}}")]
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


    [HttpDelete("{{id}}")]
    public async Task<IActionResult> DeleteRoom(string id)
    {
        FilterDefinition<Room> filter = Builders<Room>.Filter.Eq("_id", id);
        await _roomCollection.DeleteOneAsync(filter);
        return NoContent();
    }
}
