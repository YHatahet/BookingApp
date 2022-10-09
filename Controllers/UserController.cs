using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BookingApp.Models;

using MongoDB.Bson;
using MongoDB.Driver;
using BookingApp.Services;
namespace BookingApp.Controllers;

// [ApiController, Route("users"), Authorize(Roles = "Admin")]
[ApiController, Route("users")]
public class UserController : ControllerBase
{

    private readonly IMongoCollection<User> _userCollection;
    private readonly TokenService _tokenService;


    public UserController(BookingAppService bookingAppService, TokenService tokenService)
    {
        _userCollection = bookingAppService._userCollection;
        _tokenService = tokenService;
    }


    [HttpGet, Route("all/{page}/{limit}")]
    public async Task<List<User>> GetAllUsers(int page, int limit)
    {
        return await _userCollection.Find(new BsonDocument()).Skip(page * limit).Limit(limit).ToListAsync();
    }


    [HttpGet, Route("{id}")]
    public async Task<User> GetUser(string id)
    {
        return await _userCollection.Find(o => o._id == id).FirstOrDefaultAsync();
    }

    [HttpPut, Route("{id}")]
    public async Task<IActionResult> UpdateUser(string id, [FromBody] User userBody)
    {
        var foundUser = await _userCollection.Find(o => o._id == id).FirstOrDefaultAsync();
        if (foundUser == null) NotFound("User not found");
        if (userBody.username != null) foundUser.username = userBody.username;
        if (userBody.email != null) foundUser.email = userBody.email;
        if (userBody.password != null)
        {
            var newHashedPass = _tokenService.ComputeHash(userBody.password, "SHA512", null);
            foundUser.password = userBody.password;

        }
        if (userBody.role != null) foundUser.role = userBody.role;
        await _userCollection.ReplaceOneAsync(o => o._id == id, foundUser);

        //Add cookie to response header
        string token = _tokenService.CreateToken(foundUser);
        var cookieOptions = new CookieOptions { HttpOnly = true };
        Response.Cookies.Append("token", token, cookieOptions);

        return Ok(foundUser);
    }


    [HttpDelete, Route("{id}")]
    public async Task<IActionResult> DeleteUser(string id)
    {
        FilterDefinition<User> filter = Builders<User>.Filter.Eq("_id", id);
        await _userCollection.DeleteOneAsync(filter);
        return NoContent();
    }
}
