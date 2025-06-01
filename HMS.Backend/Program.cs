using HMS.Backend.Data;
using Microsoft.EntityFrameworkCore;
using DotNetEnv; // Make sure DotNetEnv is installed

var builder = WebApplication.CreateBuilder(args);

// Load environment variables from .env
DotNetEnv.Env.Load();

// Read and replace {DB_HOST} in connection string
var rawConnString = builder.Configuration.GetConnectionString("DefaultConnection") ?? "";
var dbHost = Environment.GetEnvironmentVariable("DB_HOST") ?? "";
var connString = rawConnString.Replace("{DB_HOST}", dbHost);

// Register services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register DbContext using the resolved connection string
builder.Services.AddDbContext<MyDbContext>(options =>
    options.UseSqlServer(connString));

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
