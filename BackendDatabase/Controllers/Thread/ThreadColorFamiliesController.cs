using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BackendDatabase.Data;
using ModelLibrary.Models.Thread;

namespace BackendDatabase.Controllers.Thread
{
    [Route("api/ThreadColorFamily")]
    [ApiController]
    public class ThreadColorFamiliesController : ControllerBase
    {
        private readonly BackendDatabaseContext _context;

        public ThreadColorFamiliesController(BackendDatabaseContext context)
        {
            _context = context;
        }

        // GET: api/ThreadColorFamily
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ThreadColorFamily>>> GetThreadColorFamily()
        {
          if (_context.ThreadColorFamily == null)
          {
              return NotFound();
          }
            return await _context.ThreadColorFamily.ToListAsync();
        }

        // GET: api/ThreadColorFamilies/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ThreadColorFamily>> GetThreadColorFamily(int id)
        {
          if (_context.ThreadColorFamily == null)
          {
              return NotFound();
          }
            var threadColorFamily = await _context.ThreadColorFamily.FindAsync(id);

            if (threadColorFamily == null)
            {
                return NotFound();
            }

            return threadColorFamily;
        }

        // PUT: api/ThreadColorFamilies/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutThreadColorFamily(int id, ThreadColorFamily threadColorFamily)
        {
            if (id != threadColorFamily.ID)
            {
                return BadRequest();
            }

            _context.Entry(threadColorFamily).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ThreadColorFamilyExists(id))
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

        // POST: api/ThreadColorFamilies
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ThreadColorFamily>> PostThreadColorFamily(ThreadColorFamily threadColorFamily)
        {
          if (_context.ThreadColorFamily == null)
          {
              return Problem("Entity set 'BackendDatabaseContext.ThreadColorFamily'  is null.");
          }
            _context.ThreadColorFamily.Add(threadColorFamily);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetThreadColorFamily", new { id = threadColorFamily.ID }, threadColorFamily);
        }

        // DELETE: api/ThreadColorFamilies/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteThreadColorFamily(int id)
        {
            if (_context.ThreadColorFamily == null)
            {
                return NotFound();
            }
            var threadColorFamily = await _context.ThreadColorFamily.FindAsync(id);
            if (threadColorFamily == null)
            {
                return NotFound();
            }

            _context.ThreadColorFamily.Remove(threadColorFamily);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ThreadColorFamilyExists(int id)
        {
            return (_context.ThreadColorFamily?.Any(e => e.ID == id)).GetValueOrDefault();
        }
    }
}
