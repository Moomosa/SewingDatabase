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
        public string Item { get; set; }
        public ICollection<MiscObjects> Miscs { get; set; }
    }
}
