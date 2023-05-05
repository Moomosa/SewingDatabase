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
    public class MiscItemTypesController : ControllerBase
    {
        private readonly BackendDatabaseContext _context;

        public MiscItemTypesController(BackendDatabaseContext context)
        {
            _context = context;
        }

        // GET: api/MiscItemTypes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MiscItemType>>> GetMiscItemType()
        {
          if (_context.MiscItemType == null)
          {
              return NotFound();
          }
            return await _context.MiscItemType.ToListAsync();
        }

        // GET: api/MiscItemTypes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MiscItemType>> GetMiscItemType(int id)
        {
          if (_context.MiscItemType == null)
          {
              return NotFound();
          }
            var miscItemType = await _context.MiscItemType.FindAsync(id);

            if (miscItemType == null)
            {
                return NotFound();
            }

            return miscItemType;
        }

        // PUT: api/MiscItemTypes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMiscItemType(int id, MiscItemType miscItemType)
        {
            if (id != miscItemType.ID)
            {
                return BadRequest();
            }

            _context.Entry(miscItemType).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MiscItemTypeExists(id))
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

        // POST: api/MiscItemTypes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<MiscItemType>> PostMiscItemType(MiscItemType miscItemType)
        {
          if (_context.MiscItemType == null)
          {
              return Problem("Entity set 'BackendDatabaseContext.MiscItemType'  is null.");
          }
            _context.MiscItemType.Add(miscItemType);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMiscItemType", new { id = miscItemType.ID }, miscItemType);
        }

        // DELETE: api/MiscItemTypes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMiscItemType(int id)
        {
            if (_context.MiscItemType == null)
            {
                return NotFound();
            }
            var miscItemType = await _context.MiscItemType.FindAsync(id);
            if (miscItemType == null)
            {
                return NotFound();
            }

            _context.MiscItemType.Remove(miscItemType);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MiscItemTypeExists(int id)
        {
            return (_context.MiscItemType?.Any(e => e.ID == id)).GetValueOrDefault();
        }
    }
}
