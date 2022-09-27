// using Microsoft.Extensions.Configuration;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using BookingApp.Models;

using MongoDB.Bson;
using MongoDB.Driver;

namespace BookingApp.Controllers;

[ApiController]
[Route("[controller]")]
public class HotelController : Controller //TODO Controller vs controller base? with or without view support?
{

    private IMongoCollection<Hotel> _hotelCollection;
    public HotelController(IMongoClient client)
    {
        var db = client.GetDatabase("hotelCollection");
        _hotelCollection = db.GetCollection<Hotel>("hotels");
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    [HttpGet]
    public IEnumerable<Hotel> Get()
    {
        return _hotelCollection.Find(_ => true).ToList();
    }
}
