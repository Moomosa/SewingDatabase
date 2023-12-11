using ModelLibrary.Models.Thread;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SewingModels.Models
{
    public class Thread
    {
        [Key]
        public int ID { get; set; }

		[Display(Name = "Thread Type")]
		public int ThreadTypeID { get; set; }

		[Display(Name = "Thread Type")]
		public ThreadTypes ThreadType { get; set; }

		[Display(Name = "Color Family")]
		public int ColorFamilyID { get; set; }

		[Display(Name = "Color Family")]
		public ThreadColorFamily ColorFamily { get; set; }
		
		[Display(Name = "Color")]
		public int ColorID { get; set; }

		[Display(Name = "Color")]
		public ThreadColor Color { get; set; }

        [Range(0, 999)]
        public int Quantity { get; set; }        
    }
}
