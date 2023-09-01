using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLibrary.Models.Database.DTO
{
	public class MachineDTO
	{
		public string Brand { get; set; }
		public string Model { get; set; }
		public DateTime LastServiced { get; set; }
	}
}
