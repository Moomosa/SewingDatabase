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

		// GET: api/FabricBrand/{tableName}/{userName}
		[HttpGet("{tableName}/{userName}")]
		public async Task<ActionResult<IEnumerable<FabricBrand>>> GetFabricBrandsByIds(string tableName, string userName)
		{
			List<int> ids = await _helper.GetRecordIds(tableName, userName);

			var fabricBrands = await _context.FabricBrand
				.Where(fb => ids.Contains(fb.ID))
				.ToListAsync();

			if (fabricBrands == null)
				return NotFound();

			return fabricBrands;
		}

		// GET: api/FabricBrand/5
		[HttpGet("{id}")]
		public async Task<ActionResult<FabricBrand>> GetFabricBrand(int id)
		{
			if (_context.FabricBrand == null)
			{
				return NotFound();
			}
			var fabricBrand = await _context.FabricBrand.FindAsync(id);

			if (fabricBrand == null)
			{
				return NotFound();
			}

			return fabricBrand;
		}

		// PUT: api/FabricBrand/5
		// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
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
		// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
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
			if (_context.FabricBrand == null)
				return NotFound();


			var fabricBrand = await _context.FabricBrand.FindAsync(id);
			if (fabricBrand == null)
				return NotFound();


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
