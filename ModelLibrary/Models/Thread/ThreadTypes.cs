using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SewingModels.Models
{
    public class ThreadTypes
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        public ICollection<Thread> Threads { get; set; }
    }
}
