using MongoDB.Driver;
using Microsoft.Extensions.Options;
using BookingApp.Models;

namespace BookingApp.Services;


public class BookingAppService
{
    public readonly IMongoCollection<Hotel> _hotelCollection;
    public readonly IMongoCollection<User> _userCollection;
    public readonly IMongoCollection<Review> _reviewCollection;
    public readonly IMongoCollection<Room> _roomCollection;

    public BookingAppService(IOptions<BookingAppDBSettings> bookingAppDBSettings)
    {
        MongoClient client = new MongoClient(bookingAppDBSettings.Value.ConnectionString);
        IMongoDatabase database = client.GetDatabase(bookingAppDBSettings.Value.DatabaseName);
        _hotelCollection = database.GetCollection<Hotel>(bookingAppDBSettings.Value.HotelCollectionName);
        _userCollection = database.GetCollection<User>(bookingAppDBSettings.Value.UserCollectionName);
        _reviewCollection = database.GetCollection<Review>(bookingAppDBSettings.Value.ReviewCollectionName);
        _roomCollection = database.GetCollection<Room>(bookingAppDBSettings.Value.RoomCollectionName);
    }

}

