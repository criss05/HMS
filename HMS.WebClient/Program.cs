using HMS.Shared.Repositories.Interfaces;
using HMS.Shared.Proxies.Implementations;
using System.Net.Http;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Load .env variables
DotNetEnv.Env.Load();

// Configure HttpClient
var token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIzIiwiYXVkIjoiYWNjb3VudCIsImlzcyI6ImFwcG9pbnRtZW50bWFuYWdlciIsImV4cCI6MTc0OTA0OTkwNiwiaWF0IjoxNzQ5MDQ2MzA2LCJuYmYiOjE3NDkwNDYzMDZ9.xB48ZldrA7A0wCK4Wl5SHi1Q6_YqfguJolkQRQaXCGc";
builder.Services.AddHttpClient();

// Register repositories
builder.Services.AddScoped<IDoctorRepository>(provider =>
{
    var httpClientFactory = provider.GetRequiredService<IHttpClientFactory>();
    var httpClient = httpClientFactory.CreateClient();
    return new DoctorProxy(httpClient, token);
});

builder.Services.AddScoped<IPatientRepository>(provider =>
{
    var httpClientFactory = provider.GetRequiredService<IHttpClientFactory>();
    var httpClient = httpClientFactory.CreateClient();
    return new PatientProxy(httpClient, token);
});

builder.Services.AddScoped<IMedicalRecordRepository>(provider =>
{
    var httpClientFactory = provider.GetRequiredService<IHttpClientFactory>();
    var httpClient = httpClientFactory.CreateClient();
    return new MedicalRecordProxy(httpClient, token);
});

builder.Services.AddScoped<IAppointmentRepository>(sp => {
    var httpClientFactory = sp.GetRequiredService<IHttpClientFactory>();
    var httpClient = httpClientFactory.CreateClient();
    // TODO: Get token from authentication service
    var token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIzIiwiYXVkIjoiYWNjb3VudCIsImlzcyI6ImFwcG9pbnRtZW50bWFuYWdlciIsImV4cCI6MTc0OTA0OTkwNiwiaWF0IjoxNzQ5MDQ2MzA2LCJuYmYiOjE3NDkwNDYzMDZ9.xB48ZldrA7A0wCK4Wl5SHi1Q6_YqfguJolkQRQaXCGc";
    return new AppointmentProxy(httpClient, token);
});

builder.Services.AddScoped<IScheduleRepository>(sp => {
    var httpClientFactory = sp.GetRequiredService<IHttpClientFactory>();
    var httpClient = httpClientFactory.CreateClient();
    // TODO: Get token from authentication service
    var token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIzIiwiYXVkIjoiYWNjb3VudCIsImlzcyI6ImFwcG9pbnRtZW50bWFuYWdlciIsImV4cCI6MTc0OTA0OTkwNiwiaWF0IjoxNzQ5MDQ2MzA2LCJuYmYiOjE3NDkwNDYzMDZ9.xB48ZldrA7A0wCK4Wl5SHi1Q6_YqfguJolkQRQaXCGc";
    return new ScheduleProxy(httpClient, token);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Doctor}/{action=Profile}/{id?}");

app.Run();
