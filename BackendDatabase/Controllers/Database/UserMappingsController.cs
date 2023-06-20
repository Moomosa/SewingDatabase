using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BackendDatabase.Data;
using ModelLibrary.Models.Database;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using NuGet.ContentModel;
using System.Security.Cryptography;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Dynamic;

namespace BackendDatabase.Controllers.Database
{
	[Route("api/[controller]")]
	[ApiController]
	public class UserMappingsController : ControllerBase
	{
		private readonly BackendDatabaseContext _context;

		public UserMappingsController(BackendDatabaseContext context)
		{
			_context = context;
		}

		// GET: api/UserMappings
		[HttpGet]
		public async Task<ActionResult<IEnumerable<UserMapping>>> GetUserMapping()
		{
			if (_context.UserMapping == null)
			{
				return NotFound();
			}
			return await _context.UserMapping.ToListAsync();
		}

		//This may not be neccessary anymore
		[HttpGet("{tableName}/{userName}")]
		public async Task<List<int>> GetRecordIds(string tableName, string userName)
		{
			List<int> recordIds = await _context.UserMapping
				.Where(um => um.UserId == userName && um.TableName == tableName)
				.Select(um => um.RecordId)
				.ToListAsync();

			return recordIds;
		}

		// GET: api/UserMappings/5
		[HttpGet("{id}")]
		public async Task<ActionResult<UserMapping>> GetUserMapping(int id)
		{
			if (_context.UserMapping == null)
			{
				return NotFound();
			}
			var userMapping = await _context.UserMapping.FindAsync(id);

			if (userMapping == null)
			{
				return NotFound();
			}

			return userMapping;
		}

		// PUT: api/UserMappings/5
		// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		[HttpPut("{id}")]
		public async Task<IActionResult> PutUserMapping(int id, UserMapping userMapping)
		{
			if (id != userMapping.ID)
			{
				return BadRequest();
			}

			_context.Entry(userMapping).State = EntityState.Modified;

			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!UserMappingExists(id))
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

		// POST: api/UserMappings
		// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		[HttpPost]
		public async Task<ActionResult<UserMapping>> PostUserMapping(UserMapping userMapping)
		{
			if (_context.UserMapping == null)
			{
				return Problem("Entity set 'BackendDatabaseContext.UserMapping'  is null.");
			}
			_context.UserMapping.Add(userMapping);
			await _context.SaveChangesAsync();

			return CreatedAtAction("GetUserMapping", new { id = userMapping.ID }, userMapping);
		}

		// DELETE: api/UserMappings/5
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteUserMapping(int id)
		{
			if (_context.UserMapping == null)
			{
				return NotFound();
			}
			var userMapping = await _context.UserMapping.FindAsync(id);
			if (userMapping == null)
			{
				return NotFound();
			}

			_context.UserMapping.Remove(userMapping);
			await _context.SaveChangesAsync();

			return NoContent();
		}

		private bool UserMappingExists(int id)
		{
			return (_context.UserMapping?.Any(e => e.ID == id)).GetValueOrDefault();
		}
	}
}
