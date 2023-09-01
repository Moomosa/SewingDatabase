using ModelLibrary.Models.Database.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLibrary.Models.Database
{
	public class MultiDataDTO
	{
		public List<FabricDTO> Fabrics { get; set; }
		public List<ElasticDTO> Elastics { get; set; }
		public List<ThreadDTO> Threads { get; set; }
		public List<MachineDTO> Machines { get; set; }
		public List<MiscDTO> Miscs { get; set; }
	}
}
