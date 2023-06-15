using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BackendDatabase.Data;
using SewingModels.Models;

namespace FrontEnd.Pages.Data.Fabric.Brand
{
    public class DeleteModel : PageModel
    {
        private readonly BackendDatabase.Data.BackendDatabaseContext _context;

        public DeleteModel(BackendDatabase.Data.BackendDatabaseContext context)
        {
            _context = context;
        }

        [BindProperty]
      public FabricBrand FabricBrand { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.FabricBrand == null)
            {
                return NotFound();
            }

            var fabricbrand = await _context.FabricBrand.FirstOrDefaultAsync(m => m.ID == id);

            if (fabricbrand == null)
            {
                return NotFound();
            }
            else 
            {
                FabricBrand = fabricbrand;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.FabricBrand == null)
            {
                return NotFound();
            }
            var fabricbrand = await _context.FabricBrand.FindAsync(id);

            if (fabricbrand != null)
            {
                FabricBrand = fabricbrand;
                _context.FabricBrand.Remove(FabricBrand);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
