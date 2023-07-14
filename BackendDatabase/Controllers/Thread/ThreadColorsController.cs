using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BackendDatabase.Data;
using ModelLibrary.Models.Thread;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using ModelLibrary.Models.Database;

namespace BackendDatabase.Controllers.Thread
{
    [Route("api/ThreadColor")]
    [ApiController]
    public class ThreadColorsController : ControllerBase
    {
        private readonly BackendDatabaseContext _context;
        private readonly Helper _helper;

        public ThreadColorsController(BackendDatabaseContext context, Helper helper)
        {
            _context = context;
            _helper = helper;
        }

        // GET: api/ThreadColor
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ThreadColor>>> GetThreadColor()
        {
            if (_context.ThreadColor == null)
                return NotFound();

            var threadColor = await _context.ThreadColor
                  .Include(tc => tc.ColorFamily)
                  .ToListAsync();

            if (threadColor == null)
                return NotFound();

            return threadColor;
        }

        // GET: api/ThreadColor/byIds/{tableName}/{userName}
        [HttpGet("byIds/{tableName}/{userName}")]
        public async Task<ActionResult<IEnumerable<ThreadColor>>> GetThreadColorByIds(string tableName, string userName)
        {
            List<int> ids = await _helper.GetRecordIds(tableName, userName);

            var colors = await _context.ThreadColor
                .Include(tc => tc.ColorFamily)
                .Where(tc => ids.Contains(tc.ID))
                .ToListAsync();

            if (colors == null)
                return NotFound();

            return colors;
        }

        // GET: api/ThreadColor/5/{userId}
        [HttpGet("{id}/{userId}")]
        public async Task<ActionResult<ThreadColor>> GetThreadColor(int id, string userId)
        {
            if (_context.ThreadColor == null)
                return NotFound();

            var threadColor = await _context.ThreadColor
                .Include(tc => tc.ColorFamily)
                .FirstOrDefaultAsync(tc => tc.ID == id);

            if (threadColor == null)
                return NotFound();

            if (!await _helper.IsOwnedByUser("ThreadColor", id, userId))
                return Forbid();

            return threadColor;
        }

        // PUT: api/ThreadColors/5
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
        [HttpPost]
        public async Task<ActionResult<ThreadColor>> PostThreadColor(ThreadColor threadColor, string userId)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    if (_context.ThreadColor == null)
                        return Problem("Entity set 'BackendDatabaseContext.ThreadColor'  is null.");

                    threadColor.ColorFamily = _context.ThreadColorFamily.FirstOrDefault(tc => tc.ID == threadColor.ColorFamilyID);

                    _context.ThreadColor.Add(threadColor);
                    await _context.SaveChangesAsync();

                    var userMapping = new UserMapping
                    {
                        UserId = userId,
                        TableName = "ThreadColor",
                        RecordId = threadColor.ID
                    };

                    _context.UserMapping.Add(userMapping);
                    await _context.SaveChangesAsync();

                    await transaction.CommitAsync();

                    return CreatedAtAction("GetThreadColor", new { id = threadColor.ID }, threadColor);
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }

        // DELETE: api/ThreadColors/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteThreadColor(int id)
        {
            var threadColor = await _context.ThreadColor.FindAsync(id);
            if (threadColor == null)
                return NotFound();

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    _context.ThreadColor.Remove(threadColor);
                    await _context.SaveChangesAsync();

                    var userMapping = await _context.UserMapping.FirstOrDefaultAsync(um => um.TableName == "ThreadColor" && um.RecordId == id);
                    if(userMapping != null)
                    {
                        _context.UserMapping.Remove(userMapping);
                        await _context.SaveChangesAsync();
                    }

                    await transaction.CommitAsync();

                    return NoContent();
                }
                catch(Exception)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }

        private bool ThreadColorExists(int id)
        {
            return (_context.ThreadColor?.Any(e => e.ID == id)).GetValueOrDefault();
        }
    }
}
