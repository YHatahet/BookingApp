using Microsoft.AspNetCore.Mvc;
using BookingApp.Models;



using MongoDB.Driver;
using BookingApp.Services;
namespace BookingApp.Controllers;

[ApiController, Route("auth")]
public class AuthController : ControllerBase
{

    private IMongoCollection<User> _userCollection;
    private TokenService _tokenService;
    private string _token;

    public AuthController(BookingAppService bookingAppService, TokenService tokenService)
    {
        _userCollection = bookingAppService._userCollection;
        _token = bookingAppService.token;
        _tokenService = tokenService;

    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] User newUser)
    {
        newUser.password = _tokenService.ComputeHash(newUser.password, "SHA512", null);

        await _userCollection.InsertOneAsync(newUser);
        return CreatedAtAction(nameof(Register), new { id = newUser._id }, newUser);

    }

    [HttpPost("login")]
    public async Task<ActionResult<string>> Login([FromBody] User loginUser)
    {
        var user = await _userCollection.Find(o => o.username == loginUser.username).FirstOrDefaultAsync();

        if (user == null) return BadRequest("User not found");

        if (!_tokenService.VerifyHash(loginUser.password, "SHA512", user.password)) return BadRequest("Incorrect Password");

        string token = _tokenService.CreateToken(user);

        //Add cookie to response header
        var cookieOptions = new CookieOptions { HttpOnly = true };
        Response.Cookies.Append("token", token, cookieOptions);

        return Ok(token);
    }
}

