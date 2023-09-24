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
    [Route("api/MiscItemType")]
    [ApiController]
    public class MiscItemTypesController : ControllerBase
    {
        private readonly BackendDatabaseContext _context;
        private readonly Helper _helper;

        public MiscItemTypesController(BackendDatabaseContext context, Helper helper)
        {
            _context = context;
            _helper = helper;
        }

        // GET: api/MiscItemType
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MiscItemType>>> GetMiscItemType()
        {
            if (_context.MiscItemType == null)
                return NotFound();

            return await _context.MiscItemType.ToListAsync();
        }

        // GET: api/MiscItemType/byIds/{tableName}/{userName}
        [HttpGet("byIds/{tableName}/{userName}")]
        public async Task<ActionResult<IEnumerable<MiscItemType>>> GetItemTypesByIds(string tableName, string userName)
        {
            List<int> ids = await _helper.GetRecordIds(tableName, userName);

            var itemTypes = await _context.MiscItemType
                .Where(it => ids.Contains(it.ID))
                .ToListAsync();

            if (itemTypes == null)
                return NotFound();

            return itemTypes;
        }

        // GET: api/MiscItemType/5/{userId}
        [HttpGet("{id}/{userId}")]
        public async Task<ActionResult<MiscItemType>> GetMiscItemType(int id, string userId)
        {
            if (_context.MiscItemType == null)
                return NotFound();

            var miscItemType = await _context.MiscItemType.FindAsync(id);

            if (miscItemType == null)
                return NotFound();

            if (!await _helper.IsOwnedByUser("MiscItemType", id, userId))
                return Forbid();

            return miscItemType;
        }

		// GET: api/MiscItemType/count/{userId}
		[HttpGet("count/{userId}")]
		public async Task<ActionResult<int>> GetTotalCount(string userId)
		{
			List<int> miscItemTypeRecordIds = await _helper.GetRecordIds("MiscItemType", userId);
			int count = miscItemTypeRecordIds.Count;
			return count;
		}

		// GET: api/MiscItemType/paged/{userId}/{page}/{recordsPerPage}
		[HttpGet("paged/{userId}/{page}/{recordsPerPage}")]
		public async Task<ActionResult<IEnumerable<MiscItemType>>> GetPagedMiscItemType(string userId, int page, int recordsPerPage)
		{
			List<int> miscItemTypeRecordIds = await _helper.GetRecordIds("MiscItemType", userId);

			var miscItemTypes = await _context.MiscItemType
				.Where(mit => miscItemTypeRecordIds.Contains(mit.ID))
				.Skip((page - 1) * recordsPerPage)
				.Take(recordsPerPage)
				.ToListAsync();

			return miscItemTypes;
		}

		// PUT: api/MiscItemTypes/5
		[HttpPut("{id}")]
        public async Task<IActionResult> PutMiscItemType(int id, MiscItemType miscItemType)
        {
            if (id != miscItemType.ID)
            {
                return BadRequest();
            }

            _context.Entry(miscItemType).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MiscItemTypeExists(id))
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

        // POST: api/MiscItemTypes
        [HttpPost]
        public async Task<ActionResult<MiscItemType>> PostMiscItemType(MiscItemType miscItemType, string userId)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    if (_context.MiscItemType == null)
                        return Problem("Entity set 'BackendDatabaseContext.MiscItemType'  is null.");

                    _context.MiscItemType.Add(miscItemType);
                    await _context.SaveChangesAsync();

                    var userMapping = new UserMapping
                    {
                        UserId = userId,
                        TableName = "MiscItemType",
                        RecordId = miscItemType.ID
                    };

                    _context.UserMapping.Add(userMapping);

                    await _context.SaveChangesAsync();

                    await transaction.CommitAsync();

                    return CreatedAtAction("GetMiscItemType", new { id = miscItemType.ID }, miscItemType);
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }

        // DELETE: api/MiscItemTypes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMiscItemType(int id)
        {
            if (_context.MiscItemType == null)
                return NotFound();

            var miscItemType = await _context.MiscItemType.FindAsync(id);
            if (miscItemType == null)
                return NotFound();

            bool associatedItems = _context.MiscObjects.Any(i => i.ItemTypeID == id);
            if (associatedItems)
                return BadRequest();

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    _context.MiscItemType.Remove(miscItemType);
                    await _context.SaveChangesAsync();

                    var userMapping = await _context.UserMapping.FirstOrDefaultAsync(um => um.TableName == "MiscItemType" && um.RecordId == id);
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

        private bool MiscItemTypeExists(int id)
        {
            return (_context.MiscItemType?.Any(e => e.ID == id)).GetValueOrDefault();
        }
    }
}
