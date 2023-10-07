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
        public FabricTypes FabricType { get; set; }
        public int FabricTypeID { get; set; }
        public FabricBrand FabricBrand { get; set; }
        public int FabricBrandID { get; set; }
        [Range(0.01, 9999.99)]
        public float PurchasePrice { get; set; }
		[Range(0.01, 9999.99)]
		public float Value { get; set; }
        public bool SolidOrPrint { get; set; }
        [MaxLength(50)]
        public string Appearance { get; set; }
		[Range(0.01, 9999.99)]
		public float Amount { get; set; }
    }
}
