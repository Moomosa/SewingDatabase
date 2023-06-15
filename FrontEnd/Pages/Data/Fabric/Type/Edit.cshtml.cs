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

namespace FrontEnd.Pages.Data.Fabric.Type
{
    public class EditModel : PageModel
    {
        private readonly BackendDatabase.Data.BackendDatabaseContext _context;

        public EditModel(BackendDatabase.Data.BackendDatabaseContext context)
        {
            _context = context;
        }

        [BindProperty]
        public FabricTypes FabricTypes { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.FabricTypes == null)
            {
                return NotFound();
            }

            var fabrictypes =  await _context.FabricTypes.FirstOrDefaultAsync(m => m.ID == id);
            if (fabrictypes == null)
            {
                return NotFound();
            }
            FabricTypes = fabrictypes;
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

            _context.Attach(FabricTypes).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FabricTypesExists(FabricTypes.ID))
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

        private bool FabricTypesExists(int id)
        {
          return (_context.FabricTypes?.Any(e => e.ID == id)).GetValueOrDefault();
        }
    }
}
