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
        public int ThreadTypeID { get; set; }
        public ThreadTypes ThreadType { get; set; }
        public int ColorFamilyID { get; set; }
        public ThreadColorFamily ColorFamily { get; set; }
        public int ColorID { get; set; }
        public ThreadColor Color { get; set; }
        [Range(0, 999)]
        public int Quantity { get; set; }        
    }
}
