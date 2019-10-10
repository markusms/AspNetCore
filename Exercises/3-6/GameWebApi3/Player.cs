using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameWebApi3
{
    public class Player
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Score { get; set; }
        public int Level { get; set; }
        public bool IsBanned { get; set; }
        public DateTime CreationTime { get; set; }
        public Item[] Items { get; set; }
        public Tags Tags { get; set; }

        public Player()
        {
            Id = Guid.NewGuid();
            CreationTime = DateTime.Now;
            Items = new Item[1];
            Items[0] = new Item();
        }

        public Player(NewPlayer newPlayer)
        {
            Id = Guid.NewGuid();
            CreationTime = DateTime.Now;
            Name = newPlayer.Name;
            Items = new Item[1];
            Items[0] = new Item();
        }
    }
}
