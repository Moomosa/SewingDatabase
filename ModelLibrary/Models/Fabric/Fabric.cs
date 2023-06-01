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
        public FabricBrand Brand { get; set; }
        public int FabricBrandID { get; set; }
        public float PurchasePrice { get; set; }
        public float Value { get; set; }
        public bool SolidOrPrint { get; set; }
        public string Appearance { get; set; }
        public float Amount { get; set; }
    }
}
