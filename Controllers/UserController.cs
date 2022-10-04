using Microsoft.AspNetCore.Mvc;
using BookingApp.Models;

using MongoDB.Bson;
using MongoDB.Driver;
using BookingApp.Services;
namespace BookingApp.Controllers;

[ApiController, Route("users")]
public class UserController : ControllerBase
{

    private readonly IMongoCollection<User> _userCollection;

    public UserController(BookingAppService bookingAppService)
    {
        _userCollection = bookingAppService._userCollection;
    }


    [HttpGet, Route("all/{page}/{limit}")]
    public async Task<List<User>> GetAllUsers(int page, int limit)
    {
        return await _userCollection.Find(new BsonDocument()).Skip(page * limit).Limit(limit).ToListAsync();
    }


    [HttpGet("{id}")]
    public async Task<User> GetUser(string id)
    {
        return await _userCollection.Find(o => o._id == id).FirstOrDefaultAsync();
    }

    [HttpPut("{id}")]
    public async Task<User> UpdateUser(string id, [FromBody] User userBody)
    {
        //TODO refresh token , rehash pass
        var foundUser = await _userCollection.Find(o => o._id == id).FirstOrDefaultAsync();
        if (userBody.username != null) foundUser.username = userBody.username;
        if (userBody.email != null) foundUser.email = userBody.email;
        if (userBody.password != null) foundUser.password = userBody.password;
        if (userBody.role != null) foundUser.role = userBody.role;
        await _userCollection.ReplaceOneAsync(o => o._id == id, foundUser);
        return foundUser;
    }


    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(string id)
    {
        FilterDefinition<User> filter = Builders<User>.Filter.Eq("_id", id);
        await _userCollection.DeleteOneAsync(filter);
        return NoContent();
    }
}
