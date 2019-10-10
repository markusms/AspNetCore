using System;
using System.Collections.Generic;
using System.Text;

namespace _2
{
    public static class PlayerExtensions
    {
        public static Item GetHighestLevelItem(this Player player)
        {
            Item highest = null;
            foreach(var item in player.Items)
            {
                if(highest == null)
                {
                    highest = item;
                }
                else
                {
                    if(highest.Level < item.Level)
                    {
                        highest = item;
                    }
                }
            }
            return highest;
        }
    }
}
