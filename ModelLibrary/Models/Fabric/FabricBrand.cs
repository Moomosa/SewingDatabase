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
        public string FullName { get; set; }
        public string? Website { get; set; }
        public string? AdditionalInfo { get; set; }
        public ICollection<Fabric> Fabrics { get; set; }
    }
}
