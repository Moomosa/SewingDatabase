using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SewingModels.Models;
using BackendDatabase.Areas.Identity.Data;

namespace BackendDatabase.Data
{
    public class BackendDatabaseContext : DbContext
    {
        public BackendDatabaseContext (DbContextOptions<BackendDatabaseContext> options)
            : base(options)
        {
        }
        public DbSet<SewingModels.Models.ElasticTypes>? ElasticTypes { get; set; }
        public DbSet<SewingModels.Models.Elastic>? Elastic { get; set; }
        public DbSet<SewingModels.Models.FabricBrand>? FabricBrand { get; set; }
        public DbSet<SewingModels.Models.FabricTypes>? FabricTypes { get; set; }
        public DbSet<SewingModels.Models.Fabric>? Fabric { get; set; }
        public DbSet<SewingModels.Models.Machine>? Machine { get; set; }
        public DbSet<SewingModels.Models.MiscItemType>? MiscItemType { get; set; }
        public DbSet<SewingModels.Models.MiscObjects>? MiscObjects { get; set; }
        public DbSet<SewingModels.Models.ThreadTypes>? ThreadTypes { get; set; }
        public DbSet<SewingModels.Models.Thread>? Thread { get; set; }

    }
}
