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

namespace BackendDatabase.Controllers.Misc
{
	[Route("api/MiscObjects")]
	[ApiController]
	public class MiscObjectsController : ControllerBase
	{
		private readonly BackendDatabaseContext _context;
		private readonly Helper _helper;

		public MiscObjectsController(BackendDatabaseContext context, Helper helper)
		{
			_context = context;
			_helper = helper;
		}

		// GET: api/MiscObjects
		[HttpGet]
		public async Task<ActionResult<IEnumerable<MiscObjects>>> GetMiscObjects()
		{
			if (_context.MiscObjects == null)
				return NotFound();

			var objects = await _context.MiscObjects
				.Include(o => o.ItemType)
				.ToListAsync();

			if (objects == null)
				return NotFound();

			return objects;
		}

		// GET: api/MiscObjects/byIds/{tableName}/{userName}
		[HttpGet("byIds/{tableName}/{userName}")]
		public async Task<ActionResult<IEnumerable<MiscObjects>>> GetObjectsByIds(string tableName, string userName)
		{
			List<int> ids = await _helper.GetRecordIds(tableName, userName);

			var objects = await _context.MiscObjects
				.Include(o => o.ItemType)
				.Where(o => ids.Contains(o.ID))
				.ToListAsync();

			if (objects == null)
				return NotFound();

			return objects;
		}

		// GET: api/MiscObjects/5/{userId}
		[HttpGet("{id}/{userId}")]
		public async Task<ActionResult<MiscObjects>> GetMiscObjects(int id, string userId)
		{
			if (_context.MiscObjects == null)
				return NotFound();

			var miscObjects = await _context.MiscObjects
				.Include(o => o.ItemType)
				.FirstOrDefaultAsync(o => o.ID == id);

			if (miscObjects == null)
				return NotFound();

			if (!await _helper.IsOwnedByUser("MiscObjects", id, userId))
				return Forbid();

			return miscObjects;
		}

		// GET: api/MiscObjects/count/{userId}
		[HttpGet("count/{userId}")]
		public async Task<ActionResult<int>> GetTotalCount(string userId)
		{
			List<int> miscObjectRecordIds = await _helper.GetRecordIds("MiscObjects", userId);
			int count = miscObjectRecordIds.Count;
			return count;
		}

		// GET: api/MiscObjects/paged/{userId}/{page}/{recordsPerPage}
		[HttpGet("paged/{userId}/{page}/{recordsPerPage}")]
		public async Task<ActionResult<IEnumerable<MiscObjects>>> GetPagedMiscObjects(string userId, int page, int recordsPerPage)
		{
			List<int> miscObjectRecordIds = await _helper.GetRecordIds("MiscObjects", userId);

			var miscObjects = await _context.MiscObjects
				.Include(mo => mo.ItemType)
				.OrderBy(mo => mo.ID)
				.Where(mo => miscObjectRecordIds.Contains(mo.ID))
				.Skip((page - 1) * recordsPerPage)
				.Take(recordsPerPage)
				.ToListAsync();

			return miscObjects;
		}

		// PUT: api/MiscObjects/5
		[HttpPut("{id}")]
		public async Task<IActionResult> PutMiscObjects(int id, SewingModels.Models.MiscObjects miscObjects)
		{
			if (id != miscObjects.ID)
			{
				return BadRequest();
			}

			_context.Entry(miscObjects).State = EntityState.Modified;

			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!MiscObjectsExists(id))
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

		// POST: api/MiscObjects
		[HttpPost]
		public async Task<ActionResult<MiscObjects>> PostMiscObjects(SewingModels.Models.MiscObjects miscObjects, string userId)
		{
			using (var transaction = _context.Database.BeginTransaction())
			{
				try
				{
					if (_context.MiscObjects == null)
						return Problem("Entity set 'BackendDatabaseContext.MiscObjects'  is null.");

					miscObjects.ItemType = _context.MiscItemType.FirstOrDefault(mit => mit.ID == miscObjects.ItemTypeID);

					_context.MiscObjects.Add(miscObjects);
					await _context.SaveChangesAsync();

					var userMapping = new UserMapping
					{
						UserId = userId,
						TableName = "MiscObjects",
						RecordId = miscObjects.ID
					};

					_context.UserMapping.Add(userMapping);
					await _context.SaveChangesAsync();

					await transaction.CommitAsync();

					return CreatedAtAction("GetMiscObjects", new { id = miscObjects.ID }, miscObjects);
				}
				catch (Exception)
				{
					await transaction.RollbackAsync();
					throw;
				}
			}
		}

		// DELETE: api/MiscObjects/5
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteMiscObjects(int id)
		{
			var miscObjects = await _context.MiscObjects.FindAsync(id);
			if (miscObjects == null)
				return NotFound();

			using (var transaction = _context.Database.BeginTransaction())
			{
				try
				{
					_context.MiscObjects.Remove(miscObjects);
					await _context.SaveChangesAsync();

					var userMapping = await _context.UserMapping.FirstOrDefaultAsync(um => um.TableName == "MiscObjects" && um.RecordId == id);
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

		private bool MiscObjectsExists(int id)
		{
			return (_context.MiscObjects?.Any(e => e.ID == id)).GetValueOrDefault();
		}
	}
}
