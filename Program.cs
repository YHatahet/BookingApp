using MongoDB.Driver;
using BookingApp.Models;
using BookingApp.Services;

var builder = WebApplication.CreateBuilder(args);

// fetch config from app settings
builder.Services.Configure<BookingAppDBSettings>(builder.Configuration.GetSection("BookingAppDBSettings"));
builder.Services.AddSingleton<BookingAppService>();


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.MapControllers();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.Run();
