using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GameWebApi3
{
    public class Item
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Level { get; set; }
        public ItemType ItemType { get; set; }
        public DateTime CreationTime { get; set; }

        public Item()
        {
            Id = Guid.NewGuid();
            Level = 1;
            ItemType = ItemType.SHIELD;
            CreationTime = DateTime.Now;
            Name = "testItem";
        }

        public Item(NewItem newItem)
        {
            Id = Guid.NewGuid();
            Level = newItem.Level;
            ItemType = newItem.ItemType;
            CreationTime = newItem.CreationTime;
            Name = newItem.Name;
        }
    }
}
