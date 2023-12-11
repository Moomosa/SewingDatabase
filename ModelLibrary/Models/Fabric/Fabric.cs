using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SewingModels.Models
{
    public class Fabric
    {
        [Key]
        public int ID { get; set; }

        [MaxLength(50)]
        public string Appearance { get; set; }

		[Display(Name = "Fabric Type")]
		public FabricTypes FabricType { get; set; }

		[Display(Name = "Fabric Type")]
		public int FabricTypeID { get; set; }

		[Display(Name = "Fabric Brand")]
		public FabricBrand FabricBrand { get; set; }

		[Display(Name = "Fabric Brand")]
		public int FabricBrandID { get; set; }

		[Display(Name = "Print")]
		public bool SolidOrPrint { get; set; }

		[Range(0.01, 9999.99)]
		public float Amount { get; set; }

		[Range(0.01, 9999.99)]
        [Display(Name = "Dollar Value")]
		[DisplayFormat(DataFormatString = "{0:C}")]
		public float Value { get; set; }

        [Range(0.01, 9999.99)]
        [Display(Name = "Purchase Price")]
		[DisplayFormat(DataFormatString = "{0:C}")]
		public float PurchasePrice { get; set; }
    }
}
