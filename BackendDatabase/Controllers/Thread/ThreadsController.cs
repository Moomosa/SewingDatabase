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

namespace BackendDatabase.Controllers.Thread
{
	[Route("api/Thread")]
	[ApiController]
	public class ThreadsController : ControllerBase
	{
		private readonly BackendDatabaseContext _context;
		private readonly Helper _helper;

		public ThreadsController(BackendDatabaseContext context, Helper helper)
		{
			_context = context;
			_helper = helper;
		}

		// GET: api/Threads
		[HttpGet]
		public async Task<ActionResult<IEnumerable<SewingModels.Models.Thread>>> GetThread()
		{
			if (_context.Thread == null)
				return NotFound();

			var threads = await _context.Thread
				.Include(t => t.ThreadType)
				.Include(t => t.Color)
				.Include(t => t.ColorFamily)
				.ToListAsync();

			if (threads == null)
				return NotFound();

			return threads;
		}

		// GET: api/Threads/byIds/{tableName}/{userName}
		[HttpGet("byIds/{tableName}/{userName}")]
		public async Task<ActionResult<IEnumerable<SewingModels.Models.Thread>>> GetThreadByIds(string tableName, string userName)
		{
			List<int> ids = await _helper.GetRecordIds(tableName, userName);

			var threads = await _context.Thread
				.Include(t => t.ThreadType)
				.Include(t => t.Color)
				.Include(t => t.ColorFamily)
				.ToListAsync();

			if (threads == null)
				return NotFound();

			return threads;
		}

		// GET: api/Threads/5/{userId}
		[HttpGet("{id}/{userId}")]
		public async Task<ActionResult<SewingModels.Models.Thread>> GetThread(int id, string userId)
		{
			if (_context.Thread == null)
				return NotFound();

			var thread = await _context.Thread
				.Include(t => t.ThreadType)
				.Include(t => t.Color)
				.Include(t => t.ColorFamily)
				.FirstOrDefaultAsync(t => t.ID == id);

			if (thread == null)
				return NotFound();

			if (!await _helper.IsOwnedByUser("Thread", id, userId))
				return Forbid();

			return thread;
		}

		// PUT: api/Threads/5
		[HttpPut("{id}")]
		public async Task<IActionResult> PutThread(int id, SewingModels.Models.Thread thread)
		{
			if (id != thread.ID)
			{
				return BadRequest();
			}

			_context.Entry(thread).State = EntityState.Modified;

			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!ThreadExists(id))
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

		// POST: api/Threads
		[HttpPost]
		public async Task<ActionResult<SewingModels.Models.Thread>> PostThread(SewingModels.Models.Thread thread, string userId)
		{
			using (var transaction = _context.Database.BeginTransaction())
			{
				try
				{
					if (_context.Thread == null)
						return Problem("Entity set 'BackendDatabaseContext.Thread'  is null.");

					thread.ThreadType = _context.ThreadTypes.FirstOrDefault(tt => tt.ID == thread.ThreadTypeID);
					thread.Color = _context.ThreadColor.FirstOrDefault(tc => tc.ID == thread.ColorID);
					thread.ColorFamily = _context.ThreadColorFamily.FirstOrDefault(tcf => tcf.ID == thread.ColorFamilyID);

					_context.Thread.Add(thread);
					await _context.SaveChangesAsync();

					var userMapping = new UserMapping
					{
						UserId = userId,
						TableName = "Thread",
						RecordId = thread.ID
					};

					_context.UserMapping.Add(userMapping);
					await _context.SaveChangesAsync();

					await transaction.CommitAsync();

					return CreatedAtAction("GetThread", new { id = thread.ID }, thread);
				}
				catch (Exception)
				{
					await transaction.RollbackAsync();
					throw;
				}
			}
		}

		// DELETE: api/Threads/5
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteThread(int id)
		{
			var thread = await _context.Thread.FindAsync(id);
			if (thread == null)
				return NotFound();

			using (var transaction = _context.Database.BeginTransaction())
			{
				try
				{
					_context.Thread.Remove(thread);
					await _context.SaveChangesAsync();

					var userMapping = await _context.UserMapping.FirstOrDefaultAsync(um => um.TableName == "Thread" && um.RecordId == id);
					if(userMapping != null)
					{
						_context.UserMapping.Remove(userMapping);
						await _context.SaveChangesAsync();
					}

					await transaction.CommitAsync();

					return NoContent();
				}
				catch
				{
					await transaction.RollbackAsync();
					throw;
				}
			}
		}

		private bool ThreadExists(int id)
		{
			return (_context.Thread?.Any(e => e.ID == id)).GetValueOrDefault();
		}
	}
}
