using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLibrary.Models.Thread
{
    public class ThreadColor
    {
        [Key]
        public int ID { get; set; }
        [MaxLength(25)]
        public string Color { get; set; }
        public int ColorFamilyID { get; set; }
        public ThreadColorFamily ColorFamily { get; set; }
    }
}
