using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SewingModels.Models;
using BackendDatabase.Areas.Identity.Data;
using ModelLibrary.Models.Database;
using ModelLibrary.Models.Thread;
using Microsoft.Extensions.Configuration;

namespace BackendDatabase.Data
{
    public class BackendDatabaseContext : DbContext
    {
        public BackendDatabaseContext(DbContextOptions<BackendDatabaseContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            string connectionString = configuration.GetConnectionString("BackendDatabaseContextConnection");

            optionsBuilder.UseSqlServer(connectionString);
            //base.OnConfiguring(optionsBuilder);            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserMapping>()
                .HasIndex(u => u.UserId)
                .HasDatabaseName("IX_UserId");

            modelBuilder.Entity<Fabric>()
                .HasOne(f => f.FabricBrand)
                .WithMany()
                .HasForeignKey(f => f.FabricBrandID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Fabric>()
                .HasOne(f => f.FabricType)
                .WithMany()
                .HasForeignKey(f => f.FabricTypeID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<SewingModels.Models.Thread>()
                .HasOne(t => t.ThreadType)
                .WithMany()
                .HasForeignKey(t => t.ThreadTypeID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<SewingModels.Models.Thread>()
                .HasOne(t => t.ColorFamily)
                .WithMany()
                .HasForeignKey(t => t.ColorFamilyID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<SewingModels.Models.Thread>()
                .HasOne(t => t.Color)
                .WithMany()
                .HasForeignKey(t => t.ColorID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ThreadColor>()
                .HasOne(tc => tc.ColorFamily)
                .WithMany()
                .HasForeignKey(tc => tc.ColorFamilyID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Elastic>()
                .HasOne(e => e.ElasticType)
                .WithMany()
                .HasForeignKey(e => e.ElasticTypeID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<MiscObjects>()
                .HasOne(m => m.ItemType)
                .WithMany()
                .HasForeignKey(m => m.ItemTypeID)
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<ModelLibrary.Models.Database.UserMapping>? UserMapping { get; set; }

        public DbSet<SewingModels.Models.FabricBrand>? FabricBrand { get; set; }

        public DbSet<SewingModels.Models.FabricTypes>? FabricTypes { get; set; }

        public DbSet<SewingModels.Models.Fabric>? Fabric { get; set; }

        public DbSet<SewingModels.Models.ElasticTypes>? ElasticTypes { get; set; }

        public DbSet<SewingModels.Models.Elastic>? Elastic { get; set; }

        public DbSet<SewingModels.Models.Machine>? Machine { get; set; }

        public DbSet<SewingModels.Models.MiscItemType>? MiscItemType { get; set; }

        public DbSet<SewingModels.Models.MiscObjects>? MiscObjects { get; set; }

        public DbSet<SewingModels.Models.ThreadTypes>? ThreadTypes { get; set; }

        public DbSet<SewingModels.Models.Thread>? Thread { get; set; }

        public DbSet<ModelLibrary.Models.Thread.ThreadColorFamily>? ThreadColorFamily { get; set; }

        public DbSet<ModelLibrary.Models.Thread.ThreadColor>? ThreadColor { get; set; }
    }
}
