using CarRentalManagementSystem.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllersWithViews();

// Database
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("CarManagementSystem")));

// Session
builder.Services.AddSession(options =>
{
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.IdleTimeout = TimeSpan.FromHours(4);
});

// HttpContextAccessor → _Layout.cshtml-ல use பண்ணுறது
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Middleware
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseSession(); // Session enabled
app.UseAuthorization();

// Areas (Admin, Customer)
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}");

// 🔹 Shortcuts for Customer Area
app.MapControllerRoute(
    name: "cars_shortcut",
    pattern: "Cars/{action=Index}/{id?}",
    defaults: new { area = "Customer", controller = "Cars" });

app.MapControllerRoute(
    name: "booking_shortcut",
    pattern: "Booking/{action=Index}/{id?}",
    defaults: new { area = "Customer", controller = "Booking" });

app.MapControllerRoute(
    name: "dashboard_shortcut",
    pattern: "Dashboard/{action=Index}/{id?}",
    defaults: new { area = "Customer", controller = "Dashboard" });

// 🔹 Shortcut for Admin Area
app.MapControllerRoute(
    name: "admin_shortcut",
    pattern: "Admin/{action=Index}/{id?}",
    defaults: new { area = "Admin", controller = "Dashboard" });

// Default
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
