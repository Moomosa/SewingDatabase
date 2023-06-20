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
    [Route("api/Thread")]
    [ApiController]
    public class ThreadsController : ControllerBase
    {
        private readonly BackendDatabaseContext _context;

        public ThreadsController(BackendDatabaseContext context)
        {
            _context = context;
        }

        // GET: api/Threads
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SewingModels.Models.Thread>>> GetThread()
        {
          if (_context.Thread == null)
          {
              return NotFound();
          }
            return await _context.Thread.ToListAsync();
        }

        // GET: api/Threads/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SewingModels.Models.Thread>> GetThread(int id)
        {
          if (_context.Thread == null)
          {
              return NotFound();
          }
            var thread = await _context.Thread.FindAsync(id);

            if (thread == null)
            {
                return NotFound();
            }

            return thread;
        }

        // PUT: api/Threads/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutThread(int id, SewingModels.Models.Thread thread)
        {
            if (id != thread.ID)
            {
                return BadRequest();
            }

            _context.Entry(thread).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ThreadExists(id))
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

        // POST: api/Threads
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<SewingModels.Models.Thread>> PostThread(SewingModels.Models.Thread thread)
        {
          if (_context.Thread == null)
          {
              return Problem("Entity set 'BackendDatabaseContext.Thread'  is null.");
          }
            _context.Thread.Add(thread);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetThread", new { id = thread.ID }, thread);
        }

        // DELETE: api/Threads/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteThread(int id)
        {
            if (_context.Thread == null)
            {
                return NotFound();
            }
            var thread = await _context.Thread.FindAsync(id);
            if (thread == null)
            {
                return NotFound();
            }

            _context.Thread.Remove(thread);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ThreadExists(int id)
        {
            return (_context.Thread?.Any(e => e.ID == id)).GetValueOrDefault();
        }
    }
}
