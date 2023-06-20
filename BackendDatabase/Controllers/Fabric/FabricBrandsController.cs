using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BackendDatabase.Data;
using SewingModels.Models;
using System.Security.Claims;
using ModelLibrary.Models.Database;

namespace BackendDatabase.Controllers.Fabric
{
    [Route("api/FabricBrand")]
    [ApiController]
    public class FabricBrandsController : ControllerBase
    {
        private readonly BackendDatabaseContext _context;

        public FabricBrandsController(BackendDatabaseContext context)
        {
            _context = context;
        }

        // GET: api/FabricBrands
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FabricBrand>>> GetFabricBrand()
        {
            if (_context.FabricBrand == null)
            {
                return NotFound();
            }
            return await _context.FabricBrand.ToListAsync();
        }

        // GET: api/FabricBrands/5
        [HttpGet("{id}")]
        public async Task<ActionResult<FabricBrand>> GetFabricBrand(int id)
        {
            if (_context.FabricBrand == null)
            {
                return NotFound();
            }
            var fabricBrand = await _context.FabricBrand.FindAsync(id);

            if (fabricBrand == null)
            {
                return NotFound();
            }

            return fabricBrand;
        }

        // PUT: api/FabricBrands/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFabricBrand(int id, FabricBrand fabricBrand)
        {
            if (id != fabricBrand.ID)
            {
                return BadRequest();
            }

            _context.Entry(fabricBrand).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FabricBrandExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/FabricBrands
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<FabricBrand>> PostFabricBrand(FabricBrand fabricBrand)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (_context.FabricBrand == null)
            {
                return Problem("Entity set 'BackendDatabaseContext.FabricBrand'  is null.");
            }
            _context.FabricBrand.Add(fabricBrand);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFabricBrand", new { id = fabricBrand.ID }, fabricBrand);
        }

        // DELETE: api/FabricBrands/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFabricBrand(int id)
        {
            if (_context.FabricBrand == null)
            {
                return NotFound();
            }
            var fabricBrand = await _context.FabricBrand.FindAsync(id);
            if (fabricBrand == null)
            {
                return NotFound();
            }

            _context.FabricBrand.Remove(fabricBrand);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool FabricBrandExists(int id)
        {
            return (_context.FabricBrand?.Any(e => e.ID == id)).GetValueOrDefault();
        }
    }
}
