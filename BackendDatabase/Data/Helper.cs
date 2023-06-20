using BackendDatabase.Controllers.Database;
using Microsoft.EntityFrameworkCore;

namespace BackendDatabase.Data
{
	public class Helper
	{
		private readonly BackendDatabaseContext _context;
		public Helper(BackendDatabaseContext context)
		{
			_context = context;
		}

		public async Task<List<int>> GetRecordIds(string tableName, string userName)
		{
			List<int> recordIds = await _context.UserMapping
				.Where(um => um.UserId == userName && um.TableName == tableName)
				.Select(um => um.RecordId)
				.ToListAsync();

			return recordIds;
		}

	}
}
