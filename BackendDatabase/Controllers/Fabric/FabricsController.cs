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
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Specialized;

namespace BackendDatabase.Controllers.Fabric
{
	[Route("api/Fabric")]
	[ApiController]
	public class FabricsController : ControllerBase
	{
		private readonly BackendDatabaseContext _context;
		private readonly Helper _helper;

		public FabricsController(BackendDatabaseContext context, Helper helper)
		{
			_context = context;
			_helper = helper;
		}

		// GET: api/Fabric
		[HttpGet]
		public async Task<ActionResult<IEnumerable<SewingModels.Models.Fabric>>> GetFabric()
		{
			if (_context.Fabric == null)
				return NotFound();

			var fabrics = await _context.Fabric
				.Include(f => f.FabricType)
				.Include(f => f.FabricBrand)
				.ToListAsync();

			if (fabrics == null)
				return NotFound();

			return fabrics;
		}

		// GET: api/Fabric/byIds/{tableName}/{userName}
		[HttpGet("byIds/{tableName}/{userName}")]
		public async Task<ActionResult<IEnumerable<SewingModels.Models.Fabric>>> GetFabricByIds(string tableName, string userName)
		{
			List<int> ids = await _helper.GetRecordIds(tableName, userName);

			var fabrics = await _context.Fabric
						  .Include(f => f.FabricBrand)
						  .Include(f => f.FabricType)
						  .Where(f => ids.Contains(f.ID))
						  .ToListAsync();

			if (fabrics == null)
				return NotFound();

			return fabrics;
		}

		// GET: api/Fabric/5/{userId}
		[HttpGet("{id}/{userId}")]
		public async Task<ActionResult<SewingModels.Models.Fabric>> GetFabric(int id, string userId)
		{
			if (_context.Fabric == null)
				return NotFound();

			var fabric = await _context.Fabric
						 .Include(f => f.FabricType)
						 .Include(f => f.FabricBrand)
						 .FirstOrDefaultAsync(f => f.ID == id);

			if (fabric == null)
				return NotFound();

			if (!await _helper.IsOwnedByUser("Fabric", id, userId))
				return Forbid();

			return fabric;
		}

		// GET: api/Fabric/count/{userId}
		[HttpGet("count/{userId}")]
		public async Task<ActionResult<int>> GetTotalCount(string userId)
		{
			List<int> fabricRecordIds = await _helper.GetRecordIds("Fabric", userId);
			int count = fabricRecordIds.Count;
			return count;
		}

		// GET: api/Fabric/paged/{userId}/{page}/{recordsPerPage}
		[HttpGet("paged/{userId}/{page}/{recordsPerPage}")]
		public async Task<ActionResult<IEnumerable<SewingModels.Models.Fabric>>> GetPagedFabric(string userId, int page, int recordsPerPage)
		{
			List<int> fabricRecordIds = await _helper.GetRecordIds("Fabric", userId);

			var fabrics = await _context.Fabric
				.Include(f => f.FabricBrand)
				.Include(f => f.FabricType)
				.Where(f => fabricRecordIds.Contains(f.ID))
				.Skip((page - 1) * recordsPerPage)
				.Take(recordsPerPage)
				.ToListAsync();

			return fabrics;
		}

		// PUT: api/Fabric/5
		[HttpPut("{id}")]
		public async Task<IActionResult> PutFabric(int id, SewingModels.Models.Fabric fabric)
		{
			if (id != fabric.ID)
			{
				return BadRequest();
			}

			_context.Entry(fabric).State = EntityState.Modified;

			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!FabricExists(id))
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

		// POST: api/Fabric
		[HttpPost]
		public async Task<ActionResult<SewingModels.Models.Fabric>> PostFabric(SewingModels.Models.Fabric fabric, string userId)
		{
			using (var transaction = _context.Database.BeginTransaction())
			{
				try
				{
					if (_context.Fabric == null)
						return Problem("Entity set 'BackendDatabaseContext.Fabric'  is null.");

					fabric.FabricBrand = _context.FabricBrand.FirstOrDefault(fb => fb.ID == fabric.FabricBrandID);
					fabric.FabricType = _context.FabricTypes.FirstOrDefault(ft => ft.ID == fabric.FabricTypeID);

					_context.Fabric.Add(fabric);
					await _context.SaveChangesAsync();

					var userMapping = new UserMapping
					{
						UserId = userId,
						TableName = "Fabric",
						RecordId = fabric.ID
					};

					_context.UserMapping.Add(userMapping);
					await _context.SaveChangesAsync();

					await transaction.CommitAsync();

					return CreatedAtAction("GetFabric", new { id = fabric.ID }, fabric);
				}
				catch (Exception)
				{
					await transaction.RollbackAsync();
					throw;
				}
			}
		}

		// DELETE: api/Fabric/5
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteFabric(int id)
		{
			var fabric = await _context.Fabric.FindAsync(id);
			if (fabric == null)
				return NotFound();

			using (var transaction = _context.Database.BeginTransaction())
			{
				try
				{
					_context.Fabric.Remove(fabric);
					await _context.SaveChangesAsync();

					var userMapping = await _context.UserMapping.FirstOrDefaultAsync(um => um.TableName == "Fabric" && um.RecordId == id);
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

		private bool FabricExists(int id)
		{
			return (_context.Fabric?.Any(e => e.ID == id)).GetValueOrDefault();
		}
	}
}
