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

		[Display(Name = "Elastic Type")]
		public ElasticTypes ElasticType { get; set; }

		[Display(Name = "Elastic Type")]
		public int ElasticTypeID { get; set; }

        [MaxLength(50)]
        public string Color { get; set; }

        [Range(0.01, 99.99)]
        public float Width { get; set; }

		[Range(0.01, 9999.99)]
		public float Length { get; set; }
    }
}
