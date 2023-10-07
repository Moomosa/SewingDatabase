using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SewingModels.Models
{
    public class FabricBrand
    {
        [Key]
        public int ID { get; set; }
        [MaxLength(50)]
        public string FullName { get; set; }
		[MaxLength(50)]
		public string? Website { get; set; }
		[MaxLength(60)]
		public string? AdditionalInfo { get; set; }
    }
}
