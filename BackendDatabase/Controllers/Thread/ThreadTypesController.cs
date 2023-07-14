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
using Microsoft.Extensions.Configuration.UserSecrets;

namespace BackendDatabase.Controllers.Thread
{
	[Route("api/[controller]")]
	[ApiController]
	public class ThreadTypesController : ControllerBase
	{
		private readonly BackendDatabaseContext _context;
		private readonly Helper _helper;

		public ThreadTypesController(BackendDatabaseContext context, Helper helper)
		{
			_context = context;
			_helper = helper;
		}

		// GET: api/ThreadTypes
		[HttpGet]
		public async Task<ActionResult<IEnumerable<ThreadTypes>>> GetThreadTypes()
		{
			if (_context.ThreadTypes == null)
				return NotFound();

			return await _context.ThreadTypes.ToListAsync();
		}

		// GET: api/ThreadTypes/byIds/{tableName}/{userName}
		[HttpGet("byIds/{tableName}/{userName}")]
		public async Task<ActionResult<IEnumerable<ThreadTypes>>> GetThreadTypesByIds(string tableName, string userName)
		{
			List<int> ids = await _helper.GetRecordIds(tableName, userName);

			var threadTypes = await _context.ThreadTypes
				.Where(tt => ids.Contains(tt.ID))
				.ToListAsync();

			if (threadTypes == null)
				return NotFound();

			return threadTypes;
		}

		// GET: api/ThreadTypes/5/{userId}
		[HttpGet("{id}/{userId}")]
		public async Task<ActionResult<ThreadTypes>> GetThreadTypes(int id, string userId)
		{
			if (_context.ThreadTypes == null)
			{
				return NotFound();
			}
			var threadTypes = await _context.ThreadTypes.FindAsync(id);

			if (threadTypes == null)
				return NotFound();

			if (!await _helper.IsOwnedByUser("ThreadTypes", id, userId))
				return Forbid();

			return threadTypes;
		}

		// PUT: api/ThreadTypes/5
		[HttpPut("{id}")]
		public async Task<IActionResult> PutThreadTypes(int id, ThreadTypes threadTypes)
		{
			if (id != threadTypes.ID)
			{
				return BadRequest();
			}

			_context.Entry(threadTypes).State = EntityState.Modified;

			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!ThreadTypesExists(id))
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

		// POST: api/ThreadTypes
		[HttpPost]
		public async Task<ActionResult<ThreadTypes>> PostThreadTypes(ThreadTypes threadTypes, string userId)
		{
			using (var transaction = _context.Database.BeginTransaction())
			{
				try
				{
					if (_context.ThreadTypes == null)
						return Problem("Entity set 'BackendDatabaseContext.ThreadTypes'  is null.");

					_context.ThreadTypes.Add(threadTypes);
					await _context.SaveChangesAsync();

					var userMapping = new UserMapping
					{
						UserId = userId,
						TableName = "ThreadTypes",
						RecordId = threadTypes.ID
					};

					_context.UserMapping.Add(userMapping);

					await _context.SaveChangesAsync();

					await transaction.CommitAsync();

					return CreatedAtAction("GetThreadTypes", new { id = threadTypes.ID }, threadTypes);
				}
				catch (Exception)
				{
					await transaction.RollbackAsync();
					throw;
				}
			}
		}

		// DELETE: api/ThreadTypes/5
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteThreadTypes(int id)
		{
			if (_context.ThreadTypes == null)
				return NotFound();

			var threadTypes = await _context.ThreadTypes.FindAsync(id);
			if (threadTypes == null)
				return NotFound();

			bool associatedThreads = _context.Thread.Any(t => t.ThreadTypeID == id);
			if (associatedThreads)
				return BadRequest();

			using (var transaction = _context.Database.BeginTransaction())
			{
				try
				{
					_context.ThreadTypes.Remove(threadTypes);
					await _context.SaveChangesAsync();

					var userMapping = await _context.UserMapping.FirstOrDefaultAsync(um => um.TableName == "ThreadTypes" && um.RecordId == id);
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

		private bool ThreadTypesExists(int id)
		{
			return (_context.ThreadTypes?.Any(e => e.ID == id)).GetValueOrDefault();
		}
	}
}
