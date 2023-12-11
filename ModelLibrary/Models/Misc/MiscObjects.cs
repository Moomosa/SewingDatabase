using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SewingModels.Models
{
    public class MiscObjects
    {
        [Key]
        public int ID { get; set; }

		[Display(Name = "Item Type")]
		public MiscItemType ItemType { get; set; }

		[Display(Name = "Item Type")]
		public int ItemTypeID { get; set; }

        [Range(0, 99)]
        public int Quantity { get; set; }

		[MaxLength(100)]
		[Display(Name = "Additional Notes")]
		public string? AdditionalNotes { get; set; }

		[MaxLength(25)]
		public string? Brand { get; set; }

		[MaxLength(100)]
		[Display(Name = "Specific Info")]
		public string? SpecificInfo { get; set; }

        [Range(0.01, 9999.99)]
		[DisplayFormat(DataFormatString = "{0:C}")]
		public float? Value { get; set; }

        [Range(0.01, 9999.99)]
		[DisplayFormat(DataFormatString = "{0:C}")]
		[Display(Name = "Purchase Price")]
		public float? PurchasePrice { get; set; }
    }
}
