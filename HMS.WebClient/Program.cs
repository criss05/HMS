using System.Text.Json.Serialization;
using HMS.Shared.Proxies.Implementations;
using HMS.Shared.Repositories.Interfaces;
using HMS.WebClient.Services;
using HMS.WebClient.ViewModels;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
        options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

// Configure HttpClient
builder.Services.AddHttpClient();
builder.Services.AddHttpContextAccessor();

// Add session services
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(2);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.SameSite = SameSiteMode.Lax;
});

// Register services
builder.Services.AddScoped<HMS.WebClient.Services.AuthService>();
builder.Services.AddScoped<HMS.WebClient.Services.PatientService>();
builder.Services.AddScoped<HMS.WebClient.Services.DoctorService>();
builder.Services.AddScoped<HMS.WebClient.Services.AppointmentService>();
builder.Services.AddScoped<HMS.WebClient.Services.MedicalRecordService>();
builder.Services.AddScoped<HMS.WebClient.Services.NotificationService>();
builder.Services.AddScoped<HMS.WebClient.Services.ReviewService>();

// Register repositories with proxy
builder.Services.AddScoped<IDoctorRepository>(provider =>
{
    var authService = provider.GetRequiredService<HMS.WebClient.Services.AuthService>();
    var httpClient = authService.CreateAuthorizedClient();
    return new DoctorProxy(httpClient, authService.GetToken() ?? string.Empty);
});

builder.Services.AddScoped<IPatientRepository>(provider =>
{
    var authService = provider.GetRequiredService<HMS.WebClient.Services.AuthService>();
    var httpClient = authService.CreateAuthorizedClient();
    return new PatientProxy(httpClient, authService.GetToken() ?? string.Empty);
});

builder.Services.AddScoped<IMedicalRecordRepository>(provider =>
{
    var authService = provider.GetRequiredService<HMS.WebClient.Services.AuthService>();
    var httpClient = authService.CreateAuthorizedClient();
    return new MedicalRecordProxy(httpClient, authService.GetToken() ?? string.Empty);
});

builder.Services.AddScoped<IAppointmentRepository>(provider =>
{
    var authService = provider.GetRequiredService<HMS.WebClient.Services.AuthService>();
    var httpClient = authService.CreateAuthorizedClient();
    return new AppointmentProxy(httpClient, authService.GetToken() ?? string.Empty);
});

builder.Services.AddScoped<IProcedureRepository>(provider =>
{
    var authService = provider.GetRequiredService<HMS.WebClient.Services.AuthService>();
    var httpClient = authService.CreateAuthorizedClient();
    return new ProcedureProxy(httpClient, authService.GetToken() ?? string.Empty);
});

builder.Services.AddScoped<IScheduleRepository>(provider =>
{
    var authService = provider.GetRequiredService<HMS.WebClient.Services.AuthService>();
    var httpClient = authService.CreateAuthorizedClient();
    return new ScheduleProxy(httpClient, authService.GetToken() ?? string.Empty);
});

builder.Services.AddScoped<INotificationRepository>(provider =>
{
    var authService = provider.GetRequiredService<HMS.WebClient.Services.AuthService>();
    var httpClient = authService.CreateAuthorizedClient();
    return new NotificationProxy(httpClient, authService.GetToken() ?? string.Empty);
});

builder.Services.AddScoped<IReviewRepository>(provider =>
{
    var authService = provider.GetRequiredService<HMS.WebClient.Services.AuthService>();
    var httpClient = authService.CreateAuthorizedClient();
    return new ReviewProxy(httpClient, authService.GetToken() ?? string.Empty);
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

app.UseSession();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.Run();