using Microsoft.AspNetCore.Mvc;
using BookingApp.Models;

using MongoDB.Bson;
using MongoDB.Driver;
using BookingApp.Services;
namespace BookingApp.Controllers;

[ApiController, Route("reviews")]
public class ReviewController : ControllerBase
{

    private readonly IMongoCollection<Review> _reviewCollection;
    private readonly IMongoCollection<Hotel> _hotelCollection;
    public ReviewController(BookingAppService bookingAppService)
    {
        _reviewCollection = bookingAppService._reviewCollection;
        _hotelCollection = bookingAppService._hotelCollection;
    }

    [HttpPost("create/{hotelid}")]
    public async Task<IActionResult> CreateReview(string hotelid, [FromBody] Review newReview)
    {

        Hotel foundHotel = await _hotelCollection.Find(o => o._id == hotelid).FirstOrDefaultAsync();
        if (foundHotel == null) return NotFound("No hotel found with given ID");
        if (foundHotel.rating.HasValue)
        {
            foundHotel.rating = 0;
            foundHotel.numOfRatings = 0;
        };
        if (foundHotel.numOfRatings.HasValue) foundHotel.numOfRatings = 0;
        double oldRating = foundHotel.rating.Value;
        double oldNumOfRatings = foundHotel.numOfRatings.Value;
        if (!newReview.rating.HasValue) return BadRequest("Missing rating");
        double newRating = (oldNumOfRatings * oldRating + newReview.rating.Value) / (oldNumOfRatings + 1);
        newReview._hotel = hotelid;
        // newRating._user = userid; // TODO fetch from cookie
        await _reviewCollection.InsertOneAsync(newReview);
        return CreatedAtAction(nameof(CreateReview), new { id = newReview._id }, newReview);

    }

    [HttpGet("all/{page}/{limit}")]
    public async Task<List<Review>> GetAllReviews(int page, int limit)
    {
        return await _reviewCollection.Find(new BsonDocument()).Skip(page * limit).Limit(limit).ToListAsync();
    }

    [HttpGet("{{id}}")]
    public async Task<Review> GetReview(string id)
    {
        return await _reviewCollection.Find(o => o._id == id).FirstOrDefaultAsync();
    }

    [HttpGet("{{hotelid}}")]
    public async Task<IActionResult> GetHotelReviews(string hotelid)
    {
        Hotel foundHotel = await _hotelCollection.Find(o => o._id == hotelid).FirstOrDefaultAsync();
        if (foundHotel == null) return NotFound("No hotel found with given ID");
        List<Review> foundReviews = await _reviewCollection.Find(o => o._hotel == hotelid).ToListAsync();
        return Ok(foundReviews);
    }

    [HttpPut("{{id}}")]
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


    [HttpDelete("{{id}}")]
    public async Task<IActionResult> DeleteReview(string id)
    {
        FilterDefinition<Review> filter = Builders<Review>.Filter.Eq("_id", id);
        await _reviewCollection.DeleteOneAsync(filter);
        //TODO change ratings
        return NoContent();
    }
}
