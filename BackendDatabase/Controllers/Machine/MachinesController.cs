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

namespace BackendDatabase.Controllers.Machine
{
	[Route("api/Machine")]
	[ApiController]
	public class MachinesController : ControllerBase
	{
		private readonly BackendDatabaseContext _context;
		private readonly Helper _helper;

		public MachinesController(BackendDatabaseContext context, Helper helper)
		{
			_context = context;
			_helper = helper;
		}

		// GET: api/Machines
		[HttpGet]
		public async Task<ActionResult<IEnumerable<SewingModels.Models.Machine>>> GetMachine()
		{
			if (_context.Machine == null)
				return NotFound();

			return await _context.Machine.ToListAsync();
		}

		// GET: api/Machines/byIds/{tableName}/{userName}
		[HttpGet("byIds/{tableName}/{userName}")]
		public async Task<ActionResult<IEnumerable<SewingModels.Models.Machine>>> GetMachinesByIds(string tableName, string userName)
		{
			List<int> ids = await _helper.GetRecordIds(tableName, userName);

			var machines = await _context.Machine
				.Where(m => ids.Contains(m.ID))
				.ToListAsync();

			if (machines == null)
				return NotFound();

			return machines;
		}

		// GET: api/Machines/5/{userId}
		[HttpGet("{id}/{userId}")]
		public async Task<ActionResult<SewingModels.Models.Machine>> GetMachine(int id, string userId)
		{
			if (_context.Machine == null)
				return NotFound();

			var machine = await _context.Machine.FindAsync(id);

			if (machine == null)
				return NotFound();

			if (!await _helper.IsOwnedByUser("Machine", id, userId))
				return Forbid();

			return machine;
		}

		// PUT: api/Machines/5
		[HttpPut("{id}")]
		public async Task<IActionResult> PutMachine(int id, SewingModels.Models.Machine machine)
		{
			if (id != machine.ID)
			{
				return BadRequest();
			}

			_context.Entry(machine).State = EntityState.Modified;

			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!MachineExists(id))
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

		// POST: api/Machines
		[HttpPost]
		public async Task<ActionResult<SewingModels.Models.Machine>> PostMachine(SewingModels.Models.Machine machine, string userId)
		{
			using (var transaction = _context.Database.BeginTransaction())
			{
				try
				{
					if (_context.Machine == null)
						return Problem("Entity set 'BackendDatabaseContext.Machine'  is null.");

					_context.Machine.Add(machine);
					await _context.SaveChangesAsync();

					var userMapping = new UserMapping
					{
						UserId = userId,
						TableName = "Machine",
						RecordId = machine.ID
					};

					_context.UserMapping.Add(userMapping);

					await _context.SaveChangesAsync();

					await transaction.CommitAsync();

					return CreatedAtAction("GetMachine", new { id = machine.ID }, machine);
				}
				catch (Exception)
				{
					await transaction.RollbackAsync();
					throw;
				}
			}
		}

		// DELETE: api/Machines/5
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteMachine(int id)
		{
			if (_context.Machine == null)
				return NotFound();

			var machine = await _context.Machine.FindAsync(id);
			if (machine == null)
				return NotFound();

			using (var transaction = _context.Database.BeginTransaction())
			{
				try
				{
					_context.Machine.Remove(machine);
					await _context.SaveChangesAsync();

					var userMapping = await _context.UserMapping.FirstOrDefaultAsync(um => um.TableName == "Machine" && um.RecordId == id);
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

		private bool MachineExists(int id)
		{
			return (_context.Machine?.Any(e => e.ID == id)).GetValueOrDefault();
		}
	}
}
