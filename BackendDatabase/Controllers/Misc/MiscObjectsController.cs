using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BackendDatabase.Data;
using SewingModels.Models;

namespace BackendDatabase.Controllers.Misc
{
    [Route("api/[controller]")]
    [ApiController]
    public class MiscObjectsController : ControllerBase
    {
        private readonly BackendDatabaseContext _context;

        public MiscObjectsController(BackendDatabaseContext context)
        {
            _context = context;
        }

        // GET: api/MiscObjects
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MiscObjects>>> GetMiscObjects()
        {
          if (_context.MiscObjects == null)
          {
              return NotFound();
          }
            return await _context.MiscObjects.ToListAsync();
        }

        // GET: api/MiscObjects/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MiscObjects>> GetMiscObjects(int id)
        {
          if (_context.MiscObjects == null)
          {
              return NotFound();
          }
            var miscObjects = await _context.MiscObjects.FindAsync(id);

            if (miscObjects == null)
            {
                return NotFound();
            }

            return miscObjects;
        }

        // PUT: api/MiscObjects/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMiscObjects(int id, MiscObjects miscObjects)
        {
            if (id != miscObjects.ID)
            {
                return BadRequest();
            }

            _context.Entry(miscObjects).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MiscObjectsExists(id))
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

        // POST: api/MiscObjects
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<MiscObjects>> PostMiscObjects(MiscObjects miscObjects)
        {
          if (_context.MiscObjects == null)
          {
              return Problem("Entity set 'BackendDatabaseContext.MiscObjects'  is null.");
          }
            _context.MiscObjects.Add(miscObjects);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMiscObjects", new { id = miscObjects.ID }, miscObjects);
        }

        // DELETE: api/MiscObjects/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMiscObjects(int id)
        {
            if (_context.MiscObjects == null)
            {
                return NotFound();
            }
            var miscObjects = await _context.MiscObjects.FindAsync(id);
            if (miscObjects == null)
            {
                return NotFound();
            }

            _context.MiscObjects.Remove(miscObjects);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MiscObjectsExists(int id)
        {
            return (_context.MiscObjects?.Any(e => e.ID == id)).GetValueOrDefault();
        }
    }
}
