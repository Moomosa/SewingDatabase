using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLibrary.Models.Thread
{
    public class ThreadColorFamily
    {
        [Key]
        public int ID { get; set; }

		[MaxLength(25)]
        [Display(Name = "Color Family")]
		public string ColorFamily { get; set; }
    }
}
