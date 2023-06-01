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
    [Route("api/[controller]")]
    [ApiController]
    public class ThreadColorsController : ControllerBase
    {
        private readonly BackendDatabaseContext _context;

        public ThreadColorsController(BackendDatabaseContext context)
        {
            _context = context;
        }

        // GET: api/ThreadColors
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ThreadColor>>> GetThreadColor()
        {
          if (_context.ThreadColor == null)
          {
              return NotFound();
          }
            return await _context.ThreadColor.ToListAsync();
        }

        // GET: api/ThreadColors/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ThreadColor>> GetThreadColor(int id)
        {
          if (_context.ThreadColor == null)
          {
              return NotFound();
          }
            var threadColor = await _context.ThreadColor.FindAsync(id);

            if (threadColor == null)
            {
                return NotFound();
            }

            return threadColor;
        }

        // PUT: api/ThreadColors/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutThreadColor(int id, ThreadColor threadColor)
        {
            if (id != threadColor.ID)
            {
                return BadRequest();
            }

            _context.Entry(threadColor).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ThreadColorExists(id))
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

        // POST: api/ThreadColors
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ThreadColor>> PostThreadColor(ThreadColor threadColor)
        {
          if (_context.ThreadColor == null)
          {
              return Problem("Entity set 'BackendDatabaseContext.ThreadColor'  is null.");
          }
            _context.ThreadColor.Add(threadColor);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetThreadColor", new { id = threadColor.ID }, threadColor);
        }

        // DELETE: api/ThreadColors/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteThreadColor(int id)
        {
            if (_context.ThreadColor == null)
            {
                return NotFound();
            }
            var threadColor = await _context.ThreadColor.FindAsync(id);
            if (threadColor == null)
            {
                return NotFound();
            }

            _context.ThreadColor.Remove(threadColor);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ThreadColorExists(int id)
        {
            return (_context.ThreadColor?.Any(e => e.ID == id)).GetValueOrDefault();
        }
    }
}
