using BackendDatabase.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BackendDatabase.Data;

public class BackendUserContext : IdentityDbContext<BackendDatabaseUser>
{
    public BackendUserContext(DbContextOptions<BackendUserContext> options)
        : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            string connectionString = configuration.GetConnectionString("BackendUserContextConnection");

            optionsBuilder.UseSqlServer(connectionString);
        }

        //base.OnConfiguring(optionsBuilder);
        //optionsBuilder.UseSqlServer("Server=MOOMOSASPLAYGRO\\SQLEXPRESS;Database=BackendUserDatabase;Trusted_Connection=True;");
    }


    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
    }
}
