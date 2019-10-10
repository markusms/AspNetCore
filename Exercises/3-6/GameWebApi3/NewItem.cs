using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace GameWebApi3
{
    public class NewItem
    {
        public string Name { get; set; }
        [Required]
        [Range(0, 99)]
        public int Level { get; set; }

        [Required]
        [EnumDataType(typeof(ItemType))]
        public ItemType ItemType { get; set; }
        [Required]
        [DateIsHistory]
        public DateTime CreationTime { get; set; }

        public NewItem()
        {
            CreationTime = DateTime.Now;
        }
    }
}
