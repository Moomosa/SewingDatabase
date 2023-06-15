using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BackendDatabase.Data;
using SewingModels.Models;

namespace FrontEnd.Pages.Data.Fabric.Fabric
{
    public class DetailsModel : PageModel
    {
        private readonly BackendDatabase.Data.BackendDatabaseContext _context;

        public DetailsModel(BackendDatabase.Data.BackendDatabaseContext context)
        {
            _context = context;
        }

      public SewingModels.Models.Fabric Fabric { get; set; } = default!; 

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Fabric == null)
            {
                return NotFound();
            }

            var fabric = await _context.Fabric.FirstOrDefaultAsync(m => m.ID == id);
            if (fabric == null)
            {
                return NotFound();
            }
            else 
            {
                Fabric = fabric;
            }
            return Page();
        }
    }
}
