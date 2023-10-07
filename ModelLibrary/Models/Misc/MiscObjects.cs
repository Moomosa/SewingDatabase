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
        public MiscItemType ItemType { get; set; }
        public int ItemTypeID { get; set; }
        [Range(0, 99)]
        public int Quantity { get; set; }
		[MaxLength(100)]
		public string? AdditionalNotes { get; set; }
		[MaxLength(25)]
		public string? Brand { get; set; }
		[MaxLength(100)]
		public string? SpecificInfo { get; set; }
        [Range(0.01, 9999.99)]
        public float? Value { get; set; }
        [Range(0.01, 9999.99)]
        public float? PurchasePrice { get; set; }
    }
}
