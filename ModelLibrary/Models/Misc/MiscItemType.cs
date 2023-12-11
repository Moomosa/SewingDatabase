using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SewingModels.Models
{
    public class MiscItemType
    {
        [Key]
        public int ID { get; set; }
		[MaxLength(30)]
        [Display(Name = "Item Type")]
		public string Item { get; set; }
    }
}
