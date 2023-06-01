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
    [Route("api/[controller]")]
    [ApiController]
    public class FabricTypesController : ControllerBase
    {
        private readonly BackendDatabaseContext _context;

        public FabricTypesController(BackendDatabaseContext context)
        {
            _context = context;
        }

        // GET: api/FabricTypes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FabricTypes>>> GetFabricTypes()
        {
            if (_context.FabricTypes == null)
            {
                return NotFound();
            }
            return await _context.FabricTypes.ToListAsync();
        }

        // GET: api/FabricTypes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<FabricTypes>> GetFabricTypes(int id)
        {
            if (_context.FabricTypes == null)
            {
                return NotFound();
            }
            var fabricTypes = await _context.FabricTypes.FindAsync(id);

            if (fabricTypes == null)
            {
                return NotFound();
            }

            return fabricTypes;
        }

        // PUT: api/FabricTypes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFabricTypes(int id, FabricTypes fabricTypes)
        {
            if (id != fabricTypes.ID)
            {
                return BadRequest();
            }

            _context.Entry(fabricTypes).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FabricTypesExists(id))
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

        // POST: api/FabricTypes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<FabricTypes>> PostFabricTypes(FabricTypes fabricTypes)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (_context.FabricTypes == null)
            {
                return Problem("Entity set 'BackendDatabaseContext.FabricTypes'  is null.");
            }
            _context.FabricTypes.Add(fabricTypes);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFabricTypes", new { id = fabricTypes.ID }, fabricTypes);
        }

        // DELETE: api/FabricTypes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFabricTypes(int id)
        {
            if (_context.FabricTypes == null)
            {
                return NotFound();
            }
            var fabricTypes = await _context.FabricTypes.FindAsync(id);
            if (fabricTypes == null)
            {
                return NotFound();
            }

            _context.FabricTypes.Remove(fabricTypes);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool FabricTypesExists(int id)
        {
            return (_context.FabricTypes?.Any(e => e.ID == id)).GetValueOrDefault();
        }
    }
}
