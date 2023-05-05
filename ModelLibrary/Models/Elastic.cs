using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SewingModels.Models
{
    public class Elastic
    {
        [Key]
        public int ID { get; set; }
        public string Color { get; set; }
        public float Width { get; set; }
        public int ElasticTypeID { get; set; }        
        public ElasticTypes ElasticType { get; set; }
        public float Length { get; set; }
    }
}
