using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SewingModels.Models
{
    public class MiscObjects
    {
        [Key]
        public int ID { get; set; }        
        public int Quantity { get; set; }
        public int ItemTypeID { get; set; }
        public MiscItemType ItemType { get; set; }
        public string? AdditionalNotes { get; set; }
        public string? Brand { get; set; }
        public string? SpecificInfo { get; set; }
        public float Value { get; set; }
        public float? PurchasePrice { get; set; }
    }
}
