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
	[Route("api/FabricBrand")]
	[ApiController]
	public class FabricBrandsController : ControllerBase
	{
		private readonly BackendDatabaseContext _context;
		private readonly Helper _helper;

		public FabricBrandsController(BackendDatabaseContext context, Helper helper)
		{
			_context = context;
			_helper = helper;
		}

		// GET: api/FabricBrand
		[HttpGet]
		public async Task<ActionResult<IEnumerable<FabricBrand>>> GetFabricBrand()
		{
			if (_context.FabricBrand == null)
			{
				return NotFound();
			}
			return await _context.FabricBrand.ToListAsync();
		}

		// GET: api/FabricBrand/byIds/{tableName}/{userName}
		[HttpGet("byIds/{tableName}/{userName}")]
		public async Task<ActionResult<IEnumerable<FabricBrand>>> GetFabricBrandByIds(string tableName, string userName)
		{
			List<int> ids = await _helper.GetRecordIds(tableName, userName);

			var fabricBrands = await _context.FabricBrand
				.Where(fb => ids.Contains(fb.ID))
				.ToListAsync();

			if (fabricBrands == null)
				return NotFound();

			return fabricBrands;
		}

		// GET: api/FabricBrand/5/{userId}
		[HttpGet("{id}/{userId}")]
		public async Task<ActionResult<FabricBrand>> GetFabricBrand(int id, string userId)
		{
			if (_context.FabricBrand == null)
				return NotFound();

			var fabricBrand = await _context.FabricBrand.FindAsync(id);

			if (fabricBrand == null)
				return NotFound();

			if (!await _helper.IsOwnedByUser("FabricBrand", id, userId))
				return Forbid();

			return fabricBrand;
		}

		// GET: api/FabricBrand/count/{userId}
		[HttpGet("count/{userId}")]
		public async Task<ActionResult<int>> GetTotalCount(string userId)
		{
			List<int> brandRecordIds = await _helper.GetRecordIds("FabricBrand", userId);
			int count = brandRecordIds.Count;
			return count;
		}

		// GET: api/FabricBrand/paged/{userId}/{page}/{recordsPerPage}
		[HttpGet("paged/{userId}/{page}/{recordsPerPage}")]
		public async Task<ActionResult<IEnumerable<FabricBrand>>> GetPagedFabricBrand(string userId, int page, int recordsPerPage)
		{
			List<int> fabricBrandRecordIds = await _helper.GetRecordIds("FabricBrand", userId);

			var brands = await _context.FabricBrand
				.Where(fb => fabricBrandRecordIds.Contains(fb.ID))
				.Skip((page - 1) * recordsPerPage)
				.Take(recordsPerPage)
				.ToListAsync();

			return brands;
		}

		// PUT: api/FabricBrand/5
		[HttpPut("{id}")]
		public async Task<IActionResult> PutFabricBrand(int id, FabricBrand fabricBrand)
		{
			if (id != fabricBrand.ID)
			{
				return BadRequest();
			}

			_context.Entry(fabricBrand).State = EntityState.Modified;

			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!FabricBrandExists(id))
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

		// POST: api/FabricBrand
		[HttpPost]
		public async Task<ActionResult<FabricBrand>> PostFabricBrand(FabricBrand fabricBrand, string userId)
		{
			using (var transaction = _context.Database.BeginTransaction())
			{
				try
				{
					if (_context.FabricBrand == null)
						return Problem("Entity set 'BackendDatabaseContext.FabricBrand'  is null.");
					_context.FabricBrand.Add(fabricBrand);

					await _context.SaveChangesAsync();

					var userMapping = new UserMapping
					{
						UserId = userId,
						TableName = "FabricBrand",
						RecordId = fabricBrand.ID
					};

					_context.UserMapping.Add(userMapping);

					await _context.SaveChangesAsync();

					await transaction.CommitAsync();

					return CreatedAtAction("GetFabricBrand", new { id = fabricBrand.ID }, fabricBrand);
				}
				catch (Exception)
				{
					await transaction.RollbackAsync();
					throw;
				}
			}
		}

		// DELETE: api/FabricBrand/5
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteFabricBrand(int id)
		{
			var fabricBrand = await _context.FabricBrand.FindAsync(id);
			if (fabricBrand == null)
				return NotFound();

			bool associatedFabrics = await _context.Fabric.AnyAsync(f => f.FabricBrandID == id);
			if (associatedFabrics)
				return BadRequest();

			using (var transaction = _context.Database.BeginTransaction())
			{
				try
				{
					_context.FabricBrand.Remove(fabricBrand);
					await _context.SaveChangesAsync();

					var userMapping = await _context.UserMapping.FirstOrDefaultAsync(um => um.TableName == "FabricBrand" && um.RecordId == id);
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

		private bool FabricBrandExists(int id)
		{
			return (_context.FabricBrand?.Any(e => e.ID == id)).GetValueOrDefault();
		}
	}
}
