using FrontEnd;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using BackendDatabase.Data;
using BackendDatabase.Areas.Identity.Data;
using System.Configuration;
using BackendDatabase.Controllers.Database;

var builder = WebApplication.CreateBuilder(args);
//var connectionString = builder.Configuration.GetConnectionString("BackendUserContextConnection") ?? throw new InvalidOperationException("Connection string 'BackendUserContextConnection' not found.");

builder.Services.AddDbContext<BackendDatabaseContext>(options =>
	options.UseSqlServer(builder.Configuration.GetConnectionString("BackendDatabaseContextConnection") ?? throw new InvalidOperationException("Connection string 'BackendDatabaseContextConnection' not found.")));

builder.Services.AddDbContext<BackendUserContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("BackendUserContextConnection") ?? throw new InvalidOperationException("Connection string 'BackendUserContextConnection' not found.")));

//builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
//    .AddEntityFrameworkStores<BackendUserContext>();

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
	.AddDefaultTokenProviders()
	.AddDefaultUI()
	.AddEntityFrameworkStores<BackendUserContext>();

// Add services to the container.
builder.Services.AddRazorPages()
    .AddRazorRuntimeCompilation();

builder.Services.AddSession();
builder.Services.AddScoped<ApiService>();

var configuration = builder.Configuration;
var baseUrl = configuration.GetSection("AppSettings:BaseUrl").Value;

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();
