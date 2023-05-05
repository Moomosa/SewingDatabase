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
        public string Color { get; set; }
        public int Quantity { get; set; }
        public bool MaxiLockStretch { get; set; }
        public string ColorFamily { get; set; }
    }
}
