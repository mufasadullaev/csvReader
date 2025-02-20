using Microsoft.EntityFrameworkCore;
using synelApp.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))); // fetch connection string

var app = builder.Build();
app.UseStaticFiles(); // allow static files (html,css,js,img..etc)
app.UseRouting(); // allow routing

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Employee}/{action=Index}"); // default route is /Employee/Index

app.Run();
