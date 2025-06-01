using HMS.Backend.Data;
using Microsoft.EntityFrameworkCore;
using DotNetEnv;
using HMS.Backend.Repositories.Interfaces;
using HMS.Backend.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Load .env variables
DotNetEnv.Env.Load();

// Get DB_HOST, make sure to escape backslash if needed
var dbHost = Environment.GetEnvironmentVariable("DB_HOST") ?? "localhost\\SQLEXPRESS";

// Build the connection string dynamically
var connectionString = $"Server={dbHost};Database=HMS_DB_01;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True";

// Use connection string for DbContext
builder.Services.AddDbContext<MyDbContext>(options =>
    options.UseSqlServer(connectionString));

// Other services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// DI
builder.Services.AddScoped<IUserRepository, UserRepository>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
