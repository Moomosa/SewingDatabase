using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BackendDatabase.Data;
using SewingModels.Models;

namespace FrontEnd.Pages.Data.Fabric.Type
{
    public class IndexModel : PageModel
    {
        private readonly BackendDatabase.Data.BackendDatabaseContext _context;

        public IndexModel(BackendDatabase.Data.BackendDatabaseContext context)
        {
            _context = context;
        }

        public IList<FabricTypes> FabricTypes { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.FabricTypes != null)
            {
                FabricTypes = await _context.FabricTypes.ToListAsync();
            }
        }
    }
}
