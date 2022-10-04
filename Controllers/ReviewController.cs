// using Microsoft.Extensions.Configuration;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using BookingApp.Models;

using MongoDB.Bson;
using MongoDB.Driver;
using BookingApp.Services;
namespace BookingApp.Controllers;

[ApiController, Route("reviews")]
public class ReviewController : ControllerBase
{

    private IMongoCollection<Review> _reviewCollection;
    public ReviewController(BookingAppService bookingAppService)
    {
        _reviewCollection = bookingAppService._reviewCollection;
    }

    [HttpPost("create/{hotelid}")]
    public async Task<IActionResult> CreateReview(string hotelid, [FromBody] Review newReview)
    {
        //TODO
        await _reviewCollection.InsertOneAsync(newReview);
        return CreatedAtAction(nameof(CreateReview), new { id = newReview._id }, newReview);

    }

    [HttpGet("all/{page}/{limit}")]
    public async Task<List<Review>> GetAllReviews(int page, int limit)
    {
        return await _reviewCollection.Find(new BsonDocument()).Skip(page * limit).Limit(limit).ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<Review> GetReview(string id)
    {
        return await _reviewCollection.Find(o => o._id == id).FirstOrDefaultAsync();
    }

    //TODO get hotel reviews

    [HttpPut("{id}")]
    public async Task<Review> UpdateReview(string id, [FromBody] Review reviewBody)
    {
        var foundReview = await _reviewCollection.Find(o => o._id == id).FirstOrDefaultAsync();
        if (reviewBody.rating != null) foundReview.rating = reviewBody.rating;
        if (reviewBody.review != null) foundReview.review = reviewBody.review;
        if (reviewBody._hotel != null) foundReview._hotel = reviewBody._hotel;
        if (reviewBody._user != null) foundReview._user = reviewBody._user;
        await _reviewCollection.ReplaceOneAsync(o => o._id == id, foundReview);
        return foundReview;
    }


    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteReview(string id)
    {
        FilterDefinition<Review> filter = Builders<Review>.Filter.Eq("_id", id);
        await _reviewCollection.DeleteOneAsync(filter);
        return NoContent();
    }
}
