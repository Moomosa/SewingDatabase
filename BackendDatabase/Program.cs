using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using BackendDatabase.Data;
using Microsoft.AspNetCore.Identity;
using BackendDatabase.Areas.Identity.Data;
using System.Configuration;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<BackendDatabaseContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("BackendDatabaseContextConnection") ?? throw new InvalidOperationException("Connection string 'BackendDatabaseContext' not found.")));

builder.Services.AddDbContext<BackendUserContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("BackendUserContextConnection")));

builder.Services.AddDefaultIdentity<BackendDatabaseUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<BackendUserContext>();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Sewing DB");
    });
}

app.UseHttpsRedirection();

app.UseAuthentication(); ;

app.UseAuthorization();

app.MapControllers();

app.Run();
