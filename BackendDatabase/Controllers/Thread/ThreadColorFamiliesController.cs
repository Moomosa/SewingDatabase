using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BackendDatabase.Data;
using ModelLibrary.Models.Thread;
using ModelLibrary.Models.Database;

namespace BackendDatabase.Controllers.Thread
{
    [Route("api/ThreadColorFamily")]
    [ApiController]
    public class ThreadColorFamiliesController : ControllerBase
    {
        private readonly BackendDatabaseContext _context;
        private readonly Helper _helper;

        public ThreadColorFamiliesController(BackendDatabaseContext context, Helper helper)
        {
            _context = context;
            _helper = helper;
        }

        // GET: api/ThreadColorFamily
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ThreadColorFamily>>> GetThreadColorFamily()
        {
            if (_context.ThreadColorFamily == null)
                return NotFound();

            return await _context.ThreadColorFamily.ToListAsync();
        }

        // GET: api/ThreadColorFamily/byIds/{tableName}/{userName}
        [HttpGet("byIds/{tableName}/{userName}")]
        public async Task<ActionResult<IEnumerable<ThreadColorFamily>>> GetColorFamilyByIds(string tableName, string userName)
        {
            List<int> ids = await _helper.GetRecordIds(tableName, userName);

            var colorFamily = await _context.ThreadColorFamily
                .Where(cf => ids.Contains(cf.ID))
                .ToListAsync();

            if (colorFamily == null)
                return NotFound();

            return colorFamily;
        }

        // GET: api/ThreadColorFamily/5/{userId}
        [HttpGet("{id}/{userId}")]
        public async Task<ActionResult<ThreadColorFamily>> GetThreadColorFamily(int id, string userId)
        {
            if (_context.ThreadColorFamily == null)
            {
                return NotFound();
            }
            var threadColorFamily = await _context.ThreadColorFamily.FindAsync(id);

            if (threadColorFamily == null)
                return NotFound();

            if (!await _helper.IsOwnedByUser("ThreadColorFamily", id, userId))
                return Forbid();

            return threadColorFamily;
        }

        // PUT: api/ThreadColorFamily/5
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

        // POST: api/ThreadColorFamily
        [HttpPost]
        public async Task<ActionResult<ThreadColorFamily>> PostThreadColorFamily(ThreadColorFamily threadColorFamily, string userId)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    if (_context.ThreadColorFamily == null)
                        return Problem("Entity set 'BackendDatabaseContext.ThreadColorFamily'  is null.");

                    _context.ThreadColorFamily.Add(threadColorFamily);
                    await _context.SaveChangesAsync();

                    var userMapping = new UserMapping
                    {
                        UserId = userId,
                        TableName = "ThreadColorFamily",
                        RecordId = threadColorFamily.ID
                    };

                    _context.UserMapping.Add(userMapping);

                    await _context.SaveChangesAsync();

                    await transaction.CommitAsync();

                    return CreatedAtAction("GetThreadColorFamily", new { id = threadColorFamily.ID }, threadColorFamily);
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }

        // DELETE: api/ThreadColorFamily/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteThreadColorFamily(int id)
        {
            if (_context.ThreadColorFamily == null)
                return NotFound();

            var threadColorFamily = await _context.ThreadColorFamily.FindAsync(id);
            if (threadColorFamily == null)
                return NotFound();

            bool associatedColor = _context.Thread.Any(t => t.ColorFamilyID == id);
            if (associatedColor)
                return BadRequest();

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    _context.ThreadColorFamily.Remove(threadColorFamily);
                    await _context.SaveChangesAsync();

                    var userMapping = await _context.UserMapping.FirstOrDefaultAsync(um => um.TableName == "ThreadColorFamily" && um.RecordId == id);
                    if (userMapping != null)
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

        private bool ThreadColorFamilyExists(int id)
        {
            return (_context.ThreadColorFamily?.Any(e => e.ID == id)).GetValueOrDefault();
        }
    }
}
