using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BackendDatabase.Data;
using SewingModels.Models;

namespace FrontEnd.Pages.Data.Fabric.Fabric
{
    public class EditModel : PageModel
    {
        private readonly BackendDatabase.Data.BackendDatabaseContext _context;

        public EditModel(BackendDatabase.Data.BackendDatabaseContext context)
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

            var fabric =  await _context.Fabric.FirstOrDefaultAsync(m => m.ID == id);
            if (fabric == null)
            {
                return NotFound();
            }
            Fabric = fabric;
           ViewData["FabricBrandID"] = new SelectList(_context.FabricBrand, "ID", "FullName");
           ViewData["FabricTypeID"] = new SelectList(_context.FabricTypes, "ID", "Type");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Fabric).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FabricExists(Fabric.ID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool FabricExists(int id)
        {
          return (_context.Fabric?.Any(e => e.ID == id)).GetValueOrDefault();
        }
    }
}
