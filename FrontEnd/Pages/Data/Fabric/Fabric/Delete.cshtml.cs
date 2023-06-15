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
    public class DeleteModel : PageModel
    {
        private readonly BackendDatabase.Data.BackendDatabaseContext _context;

        public DeleteModel(BackendDatabase.Data.BackendDatabaseContext context)
        {
            _context = context;
        }

        [BindProperty]
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

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.Fabric == null)
            {
                return NotFound();
            }
            var fabric = await _context.Fabric.FindAsync(id);

            if (fabric != null)
            {
                Fabric = fabric;
                _context.Fabric.Remove(Fabric);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
