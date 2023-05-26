using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ModelLibrary.Models.Database
{
    public class UserMapping
    {
        [Key]
        public int ID { get; set; }
        public string UserId { get; set; }
        public string TableName { get; set; }
        public int RecordId { get; set; }
    }
}
