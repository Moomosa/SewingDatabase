using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BackendDatabase.Data;
using SewingModels.Models;

namespace BackendDatabase.Controllers.Elastic
{
    [Route("api/[controller]")]
    [ApiController]
    public class ElasticsController : ControllerBase
    {
        private readonly BackendDatabaseContext _context;

        public ElasticsController(BackendDatabaseContext context)
        {
            _context = context;
        }

        // GET: api/Elastics
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SewingModels.Models.Elastic>>> GetElastic()
        {
          if (_context.Elastic == null)
          {
              return NotFound();
          }
            return await _context.Elastic.ToListAsync();
        }

        // GET: api/Elastics/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SewingModels.Models.Elastic>> GetElastic(int id)
        {
          if (_context.Elastic == null)
          {
              return NotFound();
          }
            var elastic = await _context.Elastic.FindAsync(id);

            if (elastic == null)
            {
                return NotFound();
            }

            return elastic;
        }

        // PUT: api/Elastics/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutElastic(int id, SewingModels.Models.Elastic elastic)
        {
            if (id != elastic.ID)
            {
                return BadRequest();
            }

            _context.Entry(elastic).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ElasticExists(id))
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

        // POST: api/Elastics
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<SewingModels.Models.Elastic>> PostElastic(SewingModels.Models.Elastic elastic)
        {
          if (_context.Elastic == null)
          {
              return Problem("Entity set 'BackendDatabaseContext.Elastic'  is null.");
          }
            _context.Elastic.Add(elastic);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetElastic", new { id = elastic.ID }, elastic);
        }

        // DELETE: api/Elastics/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteElastic(int id)
        {
            if (_context.Elastic == null)
            {
                return NotFound();
            }
            var elastic = await _context.Elastic.FindAsync(id);
            if (elastic == null)
            {
                return NotFound();
            }

            _context.Elastic.Remove(elastic);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ElasticExists(int id)
        {
            return (_context.Elastic?.Any(e => e.ID == id)).GetValueOrDefault();
        }
    }
}
