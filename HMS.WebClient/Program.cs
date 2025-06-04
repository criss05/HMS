using HMS.Shared.Repositories.Interfaces;
using HMS.Shared.Proxies.Implementations;
using System.Net.Http;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Register HttpClient
builder.Services.AddHttpClient();

// Register repositories
builder.Services.AddScoped<IDoctorRepository>(sp => {
    var httpClientFactory = sp.GetRequiredService<IHttpClientFactory>();
    var httpClient = httpClientFactory.CreateClient();
    // TODO: Get token from authentication service
    var token = "";
    return new DoctorProxy(httpClient, token);
});

builder.Services.AddScoped<IAppointmentRepository>(sp => {
    var httpClientFactory = sp.GetRequiredService<IHttpClientFactory>();
    var httpClient = httpClientFactory.CreateClient();
    // TODO: Get token from authentication service
    var token = "";
    return new AppointmentProxy(httpClient, token);
});

builder.Services.AddScoped<IScheduleRepository>(sp => {
    var httpClientFactory = sp.GetRequiredService<IHttpClientFactory>();
    var httpClient = httpClientFactory.CreateClient();
    // TODO: Get token from authentication service
    var token = "";
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
