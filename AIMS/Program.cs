using AIMS.Data;
using AIMS.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllersWithViews();

// Register IHttpContextAccessor
builder.Services.AddHttpContextAccessor(); // This registers the IHttpContextAccessor service

// Register session services
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Set session timeout
    options.Cookie.HttpOnly = true; // Make cookie inaccessible to JavaScript
    options.Cookie.IsEssential = true; // Ensure the cookie is essential for the app to function
});

// Register DataAccess with dependency injection
builder.Services.AddSingleton<ProductData>();
builder.Services.AddSingleton<UserData>();
builder.Services.AddSingleton<OrderData>();
builder.Services.AddSingleton<HistoryData>();

// Register additional services
builder.Services.AddScoped<UserService>(); // Add UserService

var app = builder.Build();

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts(); // Use HSTS for production
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Add session middleware
app.UseSession();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Index}/{id?}"); // Set Login as the default controller

app.Run();
