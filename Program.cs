using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

// Import from appsettings
var BookingAppDBSettings = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("BookingAppDBSettings");

// Add services to the container.
builder.Services.AddSingleton<IMongoClient, MongoClient>(s => new MongoClient(BookingAppDBSettings["ConnectionString"]));

builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen();

builder.Services.AddControllersWithViews();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
    app.UseSwagger();
    // app.UseSwaggerUI();
}

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.Run();
