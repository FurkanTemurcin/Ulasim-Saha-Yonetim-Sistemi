using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Portal.Models;
using Portal.Filters;
using Portal.Helpers;
using System;
using Portal.Models.Admin;
using Portal.Controllers.Admin;
using Portal.Services;
using Portal.Hubs;
using QuestPDF.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Her Proje eklendiğinde View Klasörlerini görmesi için
builder.Services.AddControllersWithViews()
  .AddRazorOptions(options =>
    {
        options.ViewLocationFormats.Add("/Views/Admin/{1}/{0}.cshtml");
        options.ViewLocationFormats.Add("/Views/New/{1}/{0}.cshtml");
        options.ViewLocationFormats.Add("/Views/USYS/{1}/{0}.cshtml");

        options.ViewLocationFormats.Add("/Views/Shared/{0}.cshtml");
    });

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<PermissionAuthorizationFilter>();
builder.Services.AddScoped<MenuHelper>();  // Add MenuHelper as a service


builder.Services.AddSignalR();
builder.Services.AddScoped<NotificationService>();
builder.Services.AddHostedService<DailyNotificationService>();

// AddScoped ekleyin, böylece UserRoleHelper uygulama boyunca kullanılabilir
builder.Services.AddScoped<UserRoleHelper>();



// Add Authentication and Cookie settings
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.LogoutPath = "/Account/Logout";
    });

    //yazdırma
// QuestPDF Lisans Ayarı
QuestPDF.Settings.License = LicenseType.Community;
// HikayeSilmeService'i arka plan hizmeti olarak ekleyin
var app = builder.Build();

// Seed the database
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    context.Database.Migrate();  // Ensure database is created and migrations are applied
}



if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

// Middleware to redirect unauthenticated users to the Login page
app.Use(async (context, next) =>
{
    if (!context.User.Identity.IsAuthenticated && !context.Request.Path.StartsWithSegments("/Account"))
    {
        context.Response.Redirect("/Account/Login");
        return;
    }
    await next();
});

app.UseEndpoints(endpoints =>
{
    endpoints.MapHub<NotificationHub>("/notificationHub");

    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
});

app.Run();
