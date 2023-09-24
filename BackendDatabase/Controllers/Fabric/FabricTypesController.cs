using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BackendDatabase.Data;
using SewingModels.Models;
using System.Security.Claims;
using ModelLibrary.Models.Database;

namespace BackendDatabase.Controllers.Fabric
{
	[Route("api/FabricTypes")]
	[ApiController]
	public class FabricTypesController : ControllerBase
	{
		private readonly BackendDatabaseContext _context;
		private readonly Helper _helper;

		public FabricTypesController(BackendDatabaseContext context, Helper helper)
		{
			_context = context;
			_helper = helper;
		}

		// GET: api/FabricTypes
		[HttpGet]
		public async Task<ActionResult<IEnumerable<FabricTypes>>> GetFabricTypes()
		{
			if (_context.FabricTypes == null)			
				return NotFound();
			
			return await _context.FabricTypes.ToListAsync();
		}

		// GET: api/FabricTypes/byIds/{tableName}/{userName}
		[HttpGet("byIds/{tableName}/{userName}")]
		public async Task<ActionResult<IEnumerable<FabricTypes>>> GetFabricTypesByIds(string tableName, string userName)
		{
			List<int> ids = await _helper.GetRecordIds(tableName, userName);

			var fabricTypes = await _context.FabricTypes
				.Where(ft => ids.Contains(ft.ID))
				.ToListAsync();

			if (fabricTypes == null)
				return NotFound();

			return fabricTypes;
		}

		// GET: api/FabricTypes/5/{userId}
		[HttpGet("{id}/{userId}")]
		public async Task<ActionResult<FabricTypes>> GetFabricTypes(int id, string userId)
		{
			if (_context.FabricTypes == null)
			{
				return NotFound();
			}
			var fabricTypes = await _context.FabricTypes.FindAsync(id);

			if (fabricTypes == null)
				return NotFound();

			//This is a check to see if the item belongs to the logged in user
			if (!await _helper.IsOwnedByUser("FabricTypes", id, userId))
				return Forbid();

			return fabricTypes;
		}

		// GET: api/FabricTypes/count/{userId}
		[HttpGet("count/{userId}")]
		public async Task<ActionResult<int>> GetTotalCount(string userId)
		{
			List<int> ftypesRecordIds = await _helper.GetRecordIds("FabricTypes", userId);
			int count = ftypesRecordIds.Count;
			return count;
		}

		// GET: api/FabricTypes/paged/{userId}/{page}/{recordsPerPage}
		[HttpGet("paged/{userId}/{page}/{recordsPerPage}")]
		public async Task<ActionResult<IEnumerable<FabricTypes>>> GetPagedTypes(string userId, int page, int recordsPerPage)
		{
			List<int> typesRecordIds = await _helper.GetRecordIds("FabricTypes", userId);

			var types = await _context.FabricTypes
				.Where(ft => typesRecordIds.Contains(ft.ID))
				.Skip((page - 1) * recordsPerPage)
				.Take(recordsPerPage)
				.ToListAsync();

			return types;
		}

		// PUT: api/FabricTypes/5
		[HttpPut("{id}")]
		public async Task<IActionResult> PutFabricTypes(int id, FabricTypes fabricTypes)
		{
			if (id != fabricTypes.ID)
			{
				return BadRequest();
			}

			_context.Entry(fabricTypes).State = EntityState.Modified;

			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!FabricTypesExists(id))
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

		// POST: api/FabricTypes
		[HttpPost]
		public async Task<ActionResult<FabricTypes>> PostFabricTypes(FabricTypes fabricTypes, string userId)
		{
			using (var transaction = _context.Database.BeginTransaction())
			{
				try
				{
					if (_context.FabricTypes == null)
						return Problem("Entity set 'BackendDatabaseContext.FabricTypes' is null.");

					_context.FabricTypes.Add(fabricTypes);
					await _context.SaveChangesAsync();

					var userMapping = new UserMapping
					{
						UserId = userId,
						TableName = "FabricTypes",
						RecordId = fabricTypes.ID
					};

					_context.UserMapping.Add(userMapping);

					await _context.SaveChangesAsync();

					await transaction.CommitAsync();

					return CreatedAtAction("GetFabricTypes", new { id = fabricTypes.ID }, fabricTypes);
				}
				catch (Exception)
				{
					await transaction.RollbackAsync();
					throw;
				}
			}
		}

		// DELETE: api/FabricTypes/5
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteFabricTypes(int id)
		{
			if (_context.FabricTypes == null)
				return NotFound();

			var fabricTypes = await _context.FabricTypes.FindAsync(id);
			if (fabricTypes == null)
				return NotFound();

			bool associatedFabrics = _context.Fabric.Any(f => f.FabricTypeID == id);
			if (associatedFabrics)
				return BadRequest();

			using (var transaction = _context.Database.BeginTransaction())
			{
				try
				{
					_context.FabricTypes.Remove(fabricTypes);
					await _context.SaveChangesAsync();

					var userMapping = await _context.UserMapping.FirstOrDefaultAsync(um => um.TableName == "FabricTypes" && um.RecordId == id);
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

		private bool FabricTypesExists(int id)
		{
			return (_context.FabricTypes?.Any(e => e.ID == id)).GetValueOrDefault();
		}
	}
}
