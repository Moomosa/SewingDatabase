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

		public async Task<bool> IsOwnedByUser(string tableName, int id, string userId)
		{
			return await _context.UserMapping
				.AnyAsync(um => um.TableName == tableName && um.RecordId == id && um.UserId == userId);
		}

		public async Task<List<int>> GetRandomIndices(string tableName, string userName, int count)
		{
			List<int> recordIds = await GetRecordIds(tableName, userName);
			Random rand = new Random();

			if (recordIds.Count <= count)
				return recordIds;

			List<int> randomIndices = new List<int>();
			HashSet<int> usedIndices = new HashSet<int>();

			while (randomIndices.Count < count)
			{
				int index = rand.Next(0, recordIds.Count);
				if (usedIndices.Add(index))
					randomIndices.Add(recordIds[index]);
			}
			return randomIndices;
		}
	}
}
