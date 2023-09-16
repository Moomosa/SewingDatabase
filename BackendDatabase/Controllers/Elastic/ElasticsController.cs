using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BackendDatabase.Data;
using SewingModels.Models;
using Microsoft.AspNetCore.Identity;
using ModelLibrary.Models.Database;

namespace BackendDatabase.Controllers.Elastic
{
	[Route("api/Elastic")]
	[ApiController]
	public class ElasticsController : ControllerBase
	{
		private readonly BackendDatabaseContext _context;
		private readonly Helper _helper;

		public ElasticsController(BackendDatabaseContext context, Helper helper)
		{
			_context = context;
			_helper = helper;
		}

		// GET: api/Elastic
		[HttpGet]
		public async Task<ActionResult<IEnumerable<SewingModels.Models.Elastic>>> GetElastic()
		{
			if (_context.Elastic == null)
				return NotFound();

			var elastics = await _context.Elastic
				.Include(e => e.ElasticType)
				.ToListAsync();

			if (elastics == null)
				return NotFound();

			return elastics;
		}

		// GET: api/byIds/Elastic/{tableName}/{userName}
		[HttpGet("byIds/{tableName}/{userName}")]
		public async Task<ActionResult<IEnumerable<SewingModels.Models.Elastic>>> GetElasticByIds(string tableName, string userName)
		{
			List<int> ids = await _helper.GetRecordIds(tableName, userName);

			var elastics = await _context.Elastic
				.Include(e => e.ElasticType)
				.Where(e => ids.Contains(e.ID))
				.ToListAsync();

			if (elastics == null)
				return NotFound();

			return elastics;
		}

		// GET: api/Elastic/5/{userId}
		[HttpGet("{id}/{userId}")]
		public async Task<ActionResult<SewingModels.Models.Elastic>> GetElastic(int id, string userId)
		{
			if (_context.Elastic == null)
				return NotFound();

			var elastic = await _context.Elastic
				.Include(e => e.ElasticType)
				.FirstOrDefaultAsync(e => e.ID == id);

			if (elastic == null)
				return NotFound();

			if (!await _helper.IsOwnedByUser("Elastic", id, userId))
				return Forbid();

			return elastic;
		}

		// GET: api/Elastic/count/{userId}
		[HttpGet("count/{userId}")]
		public async Task<ActionResult<int>> GetTotalCount(string userId)
		{
			List<int> elasticRecordIds = await _helper.GetRecordIds("Elastic", userId);
			int count = elasticRecordIds.Count;
			return count;
		}

		// GET: api/Elastic/paged/{userId}/{page}/{recordsPerPage}
		[HttpGet("paged/{userId}/{page}/{recordsPerPage}")]
		public async Task<ActionResult<IEnumerable<SewingModels.Models.Elastic>>> GetPagedElastic(string userId, int page, int recordsPerPage)
		{
			List<int> elasticRecordIds = await _helper.GetRecordIds("Elastic", userId);

			var elastics = await _context.Elastic
				.Include(e => e.ElasticType)
				.OrderBy(e => e.ID)
				.Where(e => elasticRecordIds.Contains(e.ID))
				.Skip((page - 1) * recordsPerPage)
				.Take(recordsPerPage)
				.ToListAsync();

			return elastics;
		}

		// PUT: api/Elastic/5
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
		[HttpPost]
		public async Task<ActionResult<SewingModels.Models.Elastic>> PostElastic(SewingModels.Models.Elastic elastic, string userId)
		{
			using (var transaction = _context.Database.BeginTransaction())
			{
				try
				{
					if (_context.Elastic == null)
						return Problem("Entity set 'BackendDatabaseContext.Elastic'  is null.");

					elastic.ElasticType = _context.ElasticTypes.FirstOrDefault(et => et.ID == elastic.ElasticTypeID);

					_context.Elastic.Add(elastic);
					await _context.SaveChangesAsync();

					var userMapping = new UserMapping
					{
						UserId = userId,
						TableName = "Elastic",
						RecordId = elastic.ID
					};

					_context.UserMapping.Add(userMapping);
					await _context.SaveChangesAsync();

					await transaction.CommitAsync();

					return CreatedAtAction("GetElastic", new { id = elastic.ID }, elastic);
				}
				catch (Exception)
				{
					await transaction.RollbackAsync();
					throw;
				}
			}
		}

		// DELETE: api/Elastics/5
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteElastic(int id)
		{
			var elastic = await _context.Elastic.FindAsync(id);
			if (elastic == null)
				return NotFound();

			using (var transaction = _context.Database.BeginTransaction())
			{
				try
				{
					_context.Elastic.Remove(elastic);
					await _context.SaveChangesAsync();

					var userMapping = await _context.UserMapping.FirstOrDefaultAsync(um => um.TableName == "Elastic" && um.RecordId == id);
					if (userMapping != null)
					{
						_context.UserMapping.Remove(userMapping);
						await _context.SaveChangesAsync();
					}

					await transaction.CommitAsync();

					return NoContent();
				}
				catch (Exception)
				{
					await transaction.RollbackAsync();
					throw;
				}
			}
		}

		private bool ElasticExists(int id)
		{
			return (_context.Elastic?.Any(e => e.ID == id)).GetValueOrDefault();
		}
	}
}
