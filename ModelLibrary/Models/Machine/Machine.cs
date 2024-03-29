﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SewingModels.Models
{
    public class Machine
    {
        [Key]
        public int ID { get; set; }

        [MaxLength(25)]
        public string Brand { get; set; }

		[MaxLength(25)]
		public string Model { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Purchase Date")]
        public DateTime PurchaseDate { get; set; }

        [Range(0.01, 99999.99)]
		[DisplayFormat(DataFormatString = "{0:C}")]
		[Display(Name = "Purchase Price")]
		public float? PurchasePrice { get; set; }

		[DataType(DataType.Date)]
        [Display(Name = "Last Serviced")]
		public DateTime LastServiced { get; set; }
    }
}
