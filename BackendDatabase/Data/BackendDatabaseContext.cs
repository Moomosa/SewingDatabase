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

    }
}
