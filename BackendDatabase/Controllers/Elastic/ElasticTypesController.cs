using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BackendDatabase.Data;
using SewingModels.Models;
using ModelLibrary.Models.Database;

namespace BackendDatabase.Controllers.Elastic
{
	[Route("api/ElasticTypes")]
	[ApiController]
	public class ElasticTypesController : ControllerBase
	{
		private readonly BackendDatabaseContext _context;
		private readonly Helper _helper;

		public ElasticTypesController(BackendDatabaseContext context, Helper helper)
		{
			_context = context;
			_helper = helper;
		}

		// GET: api/ElasticTypes
		[HttpGet]
		public async Task<ActionResult<IEnumerable<ElasticTypes>>> GetElasticTypes()
		{
			if (_context.ElasticTypes == null)
				return NotFound();

			return await _context.ElasticTypes.ToListAsync();
		}

		//GET: api/ElasticTypes/byIds/{tableName}/{userName}
		[HttpGet("byIds/{tableName}/{userName}")]
		public async Task<ActionResult<IEnumerable<ElasticTypes>>> GetElasticTypesByIds(string tableName, string userName)
		{
			List<int> ids = await _helper.GetRecordIds(tableName, userName);

			var elasticTypes = await _context.ElasticTypes
				.Where(et => ids.Contains(et.ID))
				.ToListAsync();

			if (elasticTypes == null)
				return NotFound();

			return elasticTypes;
		}

		// GET: api/ElasticTypes/5/{userId}
		[HttpGet("{id}/{userID}")]
		public async Task<ActionResult<ElasticTypes>> GetElasticTypes(int id, string userId)
		{
			if (_context.ElasticTypes == null)
				return NotFound();

			var elasticTypes = await _context.ElasticTypes.FindAsync(id);

			if (elasticTypes == null)
				return NotFound();

			if (!await _helper.IsOwnedByUser("ElasticTypes", id, userId))
				return Forbid();

			return elasticTypes;
		}

		// PUT: api/ElasticTypes/5
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
		[HttpPost]
		public async Task<ActionResult<ElasticTypes>> PostElasticTypes(ElasticTypes elasticTypes, string userId)
		{
			using (var transaction = _context.Database.BeginTransaction())
			{
				try
				{
					if (_context.ElasticTypes == null)
						return Problem("Entity set 'BackendDatabaseContext.ElasticTypes'  is null.");

					_context.ElasticTypes.Add(elasticTypes);
					await _context.SaveChangesAsync();

					var userMapping = new UserMapping
					{
						UserId = userId,
						TableName = "ElasticTypes",
						RecordId = elasticTypes.ID
					};

					_context.UserMapping.Add(userMapping);

					await _context.SaveChangesAsync();

					await transaction.CommitAsync();

					return CreatedAtAction("GetElasticTypes", new { id = elasticTypes.ID }, elasticTypes);
				}
				catch (Exception)
				{
					await transaction.RollbackAsync();
					throw;
				}
			}
		}

		// DELETE: api/ElasticTypes/5
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteElasticTypes(int id)
		{
			if (_context.ElasticTypes == null)
				return NotFound();

			var elasticTypes = await _context.ElasticTypes.FindAsync(id);
			if (elasticTypes == null)
				return NotFound();

			bool associatedElastics = _context.Elastic.Any(e => e.ElasticTypeID == id);
			if (associatedElastics)
				return BadRequest();

			using (var transaction = _context.Database.BeginTransaction())
			{
				try
				{
					_context.ElasticTypes.Remove(elasticTypes);
					await _context.SaveChangesAsync();

					var userMapping = await _context.UserMapping.FirstOrDefaultAsync(um => um.TableName == "ElasticTypes" && um.RecordId == id);
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

		private bool ElasticTypesExists(int id)
		{
			return (_context.ElasticTypes?.Any(e => e.ID == id)).GetValueOrDefault();
		}
	}
}
