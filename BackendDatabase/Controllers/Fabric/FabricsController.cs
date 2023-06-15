using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BackendDatabase.Data;
using SewingModels.Models;
using ModelLibrary.Models.Database;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace BackendDatabase.Controllers.Fabric
{
    [Route("api/[controller]")]
    [ApiController]
    public class FabricsController : ControllerBase
    {
        private readonly BackendDatabaseContext _context;

        public FabricsController(BackendDatabaseContext context)
        {
            _context = context;
        }

        // GET: api/Fabrics
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SewingModels.Models.Fabric>>> GetFabric()
        {
            var fabrics = await _context.Fabric
                .Include(f => f.FabricType)
                .Include(f => f.FabricBrand)
                .ToListAsync();

            if (fabrics == null)
            {
                return NotFound();
            }
            return fabrics;
        }

        // GET: api/Fabrics/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SewingModels.Models.Fabric>> GetFabric(int id)
        {
            if (_context.Fabric == null)
            {
                return NotFound();
            }
            var fabric = await _context.Fabric.FindAsync(id);

            if (fabric == null)
            {
                return NotFound();
            }

            return fabric;
        }

        // PUT: api/Fabrics/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFabric(int id, SewingModels.Models.Fabric fabric)
        {
            if (id != fabric.ID)
            {
                return BadRequest();
            }

            _context.Entry(fabric).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FabricExists(id))
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

        // POST: api/Fabrics
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<SewingModels.Models.Fabric>> PostFabric(SewingModels.Models.Fabric fabric)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (_context.Fabric == null)
            {
                return Problem("Entity set 'BackendDatabaseContext.Fabric'  is null.");
            }
            _context.Fabric.Add(fabric);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFabric", new { id = fabric.ID }, fabric);
        }

        // DELETE: api/Fabrics/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFabric(int id)
        {
            if (_context.Fabric == null)
            {
                return NotFound();
            }
            var fabric = await _context.Fabric.FindAsync(id);
            if (fabric == null)
            {
                return NotFound();
            }

            _context.Fabric.Remove(fabric);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool FabricExists(int id)
        {
            return (_context.Fabric?.Any(e => e.ID == id)).GetValueOrDefault();
        }
    }
}
