using BackendDatabase.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models.Database;
using ModelLibrary.Models.Database.DTO;
using ModelLibrary.Models.Thread;
using SewingModels.Models;

namespace BackendDatabase.Controllers.Database
{
	[Route("api/Multi")]
	[ApiController]
	public class MultiDataController : ControllerBase
	{
		private readonly BackendDatabaseContext _context;
		private readonly Helper _helper;

		public MultiDataController(BackendDatabaseContext context, Helper helper)
		{
			_context = context;
			_helper = helper;
		}

		// GET: api/Multi/{userName}/{count}
		[HttpGet("{userName}/{count}")]
		public async Task<ActionResult<MultiDataDTO>> GetMultiData(string userName, int count)
		{
			//Create the id lists that the DTOs will look at
			List<int> fabricIds = await _helper.GetRandomIndices("Fabric", userName, count);
			List<int> elasticIds = await _helper.GetRandomIndices("Elastic", userName, count);
			List<int> threadIds = await _helper.GetRandomIndices("Thread", userName, count);
			List<int> machineIds = await _helper.GetRandomIndices("Machine", userName, count);
			List<int> miscIds = await _helper.GetRandomIndices("MiscObjects", userName, count);

			//code to gather data for the DTO lists
			var fabrics = await _context.Fabric
				.Include(f => f.FabricType)
				.Where(f => fabricIds.Contains(f.ID))
				.Select(f => new FabricDTO
				{
					Appearance = f.Appearance,
					Amount = f.Amount,
					FabricType = f.FabricType.Type,
				})
				.ToListAsync();

			var elastics = await _context.Elastic
				.Include(e => e.ElasticType)
				.Where(e => elasticIds.Contains(e.ID))
				.Select(e => new ElasticDTO
				{
					ElasticType = e.ElasticType.Type,
					Color = e.Color,
					Width = e.Width,
					Length = e.Length
				})
				.ToListAsync();

			var threads = await _context.Thread
				.Include(t => t.ThreadType)
				.Include(t => t.Color)
				.Where(t => threadIds.Contains(t.ID))
				.Select(t => new ThreadDTO
				{
					ThreadType = t.ThreadType.Name,
					ThreadColor = t.Color.Color,
					Quantity = t.Quantity
				})
				.ToListAsync();

			var machines = await _context.Machine
				.Where(m => machineIds.Contains(m.ID))
				.Select(m => new MachineDTO
				{
					Brand = m.Brand,
					Model = m.Model,
					LastServiced = m.LastServiced
				})
				.ToListAsync();

			var miscs = await _context.MiscObjects
				.Include(i => i.ItemType)
				.Where(i => miscIds.Contains(i.ID))
				.Select(i => new MiscDTO
				{
					ItemType = i.ItemType.Item,
					SpecificInfo = i.SpecificInfo
				})
				.ToListAsync();

			var multiData = new MultiDataDTO
			{
				Fabrics = fabrics,
				Threads = threads,
				Elastics = elastics,
				Machines = machines,
				Miscs = miscs
			};

			return Ok(multiData);
		}

	}
}
