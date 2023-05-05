using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BackendDatabase.Data;
using SewingModels.Models;

namespace BackendDatabase.Controllers.Thread
{
    [Route("api/[controller]")]
    [ApiController]
    public class ThreadTypesController : ControllerBase
    {
        private readonly BackendDatabaseContext _context;

        public ThreadTypesController(BackendDatabaseContext context)
        {
            _context = context;
        }

        // GET: api/ThreadTypes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ThreadTypes>>> GetThreadTypes()
        {
          if (_context.ThreadTypes == null)
          {
              return NotFound();
          }
            return await _context.ThreadTypes.ToListAsync();
        }

        // GET: api/ThreadTypes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ThreadTypes>> GetThreadTypes(int id)
        {
          if (_context.ThreadTypes == null)
          {
              return NotFound();
          }
            var threadTypes = await _context.ThreadTypes.FindAsync(id);

            if (threadTypes == null)
            {
                return NotFound();
            }

            return threadTypes;
        }

        // PUT: api/ThreadTypes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutThreadTypes(int id, ThreadTypes threadTypes)
        {
            if (id != threadTypes.ID)
            {
                return BadRequest();
            }

            _context.Entry(threadTypes).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ThreadTypesExists(id))
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

        // POST: api/ThreadTypes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ThreadTypes>> PostThreadTypes(ThreadTypes threadTypes)
        {
          if (_context.ThreadTypes == null)
          {
              return Problem("Entity set 'BackendDatabaseContext.ThreadTypes'  is null.");
          }
            _context.ThreadTypes.Add(threadTypes);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetThreadTypes", new { id = threadTypes.ID }, threadTypes);
        }

        // DELETE: api/ThreadTypes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteThreadTypes(int id)
        {
            if (_context.ThreadTypes == null)
            {
                return NotFound();
            }
            var threadTypes = await _context.ThreadTypes.FindAsync(id);
            if (threadTypes == null)
            {
                return NotFound();
            }

            _context.ThreadTypes.Remove(threadTypes);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ThreadTypesExists(int id)
        {
            return (_context.ThreadTypes?.Any(e => e.ID == id)).GetValueOrDefault();
        }
    }
}
