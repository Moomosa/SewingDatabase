using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SewingModels.Models
{
    public class ElasticTypes
    {
        public int ID { get; set; }
        [MaxLength(25)]
        public string Type { get; set; }
    }
}
