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
    public class DetailsModel : PageModel
    {
        private readonly BackendDatabase.Data.BackendDatabaseContext _context;

        public DetailsModel(BackendDatabase.Data.BackendDatabaseContext context)
        {
            _context = context;
        }

      public FabricTypes FabricTypes { get; set; } = default!; 

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.FabricTypes == null)
            {
                return NotFound();
            }

            var fabrictypes = await _context.FabricTypes.FirstOrDefaultAsync(m => m.ID == id);
            if (fabrictypes == null)
            {
                return NotFound();
            }
            else 
            {
                FabricTypes = fabrictypes;
            }
            return Page();
        }
    }
}
