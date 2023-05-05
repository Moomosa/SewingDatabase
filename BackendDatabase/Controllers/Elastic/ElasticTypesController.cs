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
    public class ElasticTypesController : ControllerBase
    {
        private readonly BackendDatabaseContext _context;

        public ElasticTypesController(BackendDatabaseContext context)
        {
            _context = context;
        }

        // GET: api/ElasticTypes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ElasticTypes>>> GetElasticTypes()
        {
          if (_context.ElasticTypes == null)
          {
              return NotFound();
          }
            return await _context.ElasticTypes.ToListAsync();
        }

        // GET: api/ElasticTypes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ElasticTypes>> GetElasticTypes(int id)
        {
          if (_context.ElasticTypes == null)
          {
              return NotFound();
          }
            var elasticTypes = await _context.ElasticTypes.FindAsync(id);

            if (elasticTypes == null)
            {
                return NotFound();
            }

            return elasticTypes;
        }

        // PUT: api/ElasticTypes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutElasticTypes(int id, ElasticTypes elasticTypes)
        {
            if (id != elasticTypes.ID)
            {
                return BadRequest();
            }

            _context.Entry(elasticTypes).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ElasticTypesExists(id))
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

        // POST: api/ElasticTypes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ElasticTypes>> PostElasticTypes(ElasticTypes elasticTypes)
        {
          if (_context.ElasticTypes == null)
          {
              return Problem("Entity set 'BackendDatabaseContext.ElasticTypes'  is null.");
          }
            _context.ElasticTypes.Add(elasticTypes);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetElasticTypes", new { id = elasticTypes.ID }, elasticTypes);
        }

        // DELETE: api/ElasticTypes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteElasticTypes(int id)
        {
            if (_context.ElasticTypes == null)
            {
                return NotFound();
            }
            var elasticTypes = await _context.ElasticTypes.FindAsync(id);
            if (elasticTypes == null)
            {
                return NotFound();
            }

            _context.ElasticTypes.Remove(elasticTypes);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ElasticTypesExists(int id)
        {
            return (_context.ElasticTypes?.Any(e => e.ID == id)).GetValueOrDefault();
        }
    }
}
