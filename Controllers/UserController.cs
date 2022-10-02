// using Microsoft.Extensions.Configuration;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using BookingApp.Models;

using MongoDB.Bson;
using MongoDB.Driver;

namespace BookingApp.Controllers;

[ApiController, Route("users")]
public class UserController : ControllerBase
{

    private IMongoCollection<User> _userCollection;

    public UserController(IMongoClient client)
    {
        var db = client.GetDatabase("hotelCollection");
        _userCollection = db.GetCollection<User>("users");
    }


    [HttpPost, Route("register")]
    public User RegisterNewUser()
    {
        // return _userCollection.Find(_ => true).ToList();
    }


    [HttpPost, Route("login")]
    public User Login()
    {
        // return _userCollection.Find(_ => true).ToList();
    }

    [HttpGet]
    public IEnumerable<User> Get()
    {
        return _userCollection.Find(_ => true).ToList();
    }
}
