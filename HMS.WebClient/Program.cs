using HMS.Shared.Repositories.Interfaces;
using HMS.Shared.Proxies.Implementations;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Configure HttpClient
builder.Services.AddHttpClient();

// TODO: Get token from authentication service
var token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIzIiwiYXVkIjoiYWNjb3VudCIsImlzcyI6ImFwcG9pbnRtZW50bWFuYWdlciIsImV4cCI6MTc0OTA0OTkwNiwiaWF0IjoxNzQ5MDQ2MzA2LCJuYmYiOjE3NDkwNDYzMDZ9.xB48ZldrA7A0wCK4Wl5SHi1Q6_YqfguJolkQRQaXCGc";

// Register repositories
builder.Services.AddScoped<IDoctorRepository>(provider =>
{
    var httpClient = provider.GetRequiredService<IHttpClientFactory>().CreateClient();
    return new DoctorProxy(httpClient, token);
});

builder.Services.AddScoped<IPatientRepository>(provider =>
{
    var httpClient = provider.GetRequiredService<IHttpClientFactory>().CreateClient();
    return new PatientProxy(httpClient, token);
});

builder.Services.AddScoped<IMedicalRecordRepository>(provider =>
{
    var httpClient = provider.GetRequiredService<IHttpClientFactory>().CreateClient();
    return new MedicalRecordProxy(httpClient, token);
});

builder.Services.AddScoped<IAppointmentRepository>(provider =>
{
    var httpClient = provider.GetRequiredService<IHttpClientFactory>().CreateClient();
    return new AppointmentProxy(httpClient, token);
});

builder.Services.AddScoped<IScheduleRepository>(provider =>
{
    var httpClient = provider.GetRequiredService<IHttpClientFactory>().CreateClient();
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
