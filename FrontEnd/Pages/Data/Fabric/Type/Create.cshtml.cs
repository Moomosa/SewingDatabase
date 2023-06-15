using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BackendDatabase.Data;
using SewingModels.Models;

namespace FrontEnd.Pages.Data.Fabric.Type
{
    public class CreateModel : PageModel
    {
        private readonly BackendDatabase.Data.BackendDatabaseContext _context;

        public CreateModel(BackendDatabase.Data.BackendDatabaseContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public FabricTypes FabricTypes { get; set; } = default!;
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
          if (!ModelState.IsValid || _context.FabricTypes == null || FabricTypes == null)
            {
                return Page();
            }

            _context.FabricTypes.Add(FabricTypes);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
