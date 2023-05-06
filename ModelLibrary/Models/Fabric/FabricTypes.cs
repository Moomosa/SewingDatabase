using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SewingModels.Models
{
    public class FabricTypes
    {
        [Key]
        public int ID { get; set; }
        public string Type { get; set; }
        public string? Content { get; set; }        
        public ICollection<Fabric> Fabrics { get; set; }
    }
}
