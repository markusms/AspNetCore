using System;
using System.Collections.Generic;
using System.Linq;

namespace _2
{
    class Program
    {
        static void Main(string[] args)
        {
            UniqueGuidCreator();
        }

        public static void UniqueGuidCreator()
        {
            // 1.
            Console.WriteLine("1.");
            Guid guid1 = Guid.NewGuid();
            Guid guid2 = Guid.NewGuid();

            List<Player> playerList = new List<Player>();
            List<Player> playerListToDelete = new List<Player>();
            Dictionary<Guid, Player> uniquePlayers = new Dictionary<Guid, Player>();

            for (int i = 0; i < 1000000; i++)
            {
                var player = new Player();
                player.Id = Guid.NewGuid();
                playerList.Add(player);
            }

            foreach(var player in playerList)
            {
                if(!uniquePlayers.ContainsKey(player.Id))
                {
                    uniquePlayers.Add(player.Id, player);
                }
                else
                {
                    playerListToDelete.Add(player);
                }
            }
            foreach(var player in playerListToDelete)
            {
                foreach (var playa in playerList.Reverse<Player>())
                {
                    if (player == playa)
                    {
                        playerList.Remove(playa);
                    }
                }
            }

            // 2.
            Console.WriteLine("2.");
            var playerExtensionTest = new Player();
            List<Item> extensionItemList = new List<Item>();
            playerExtensionTest.Items = extensionItemList;
            var item1 = new Item();
            item1.Level = 10;
            var item2 = new Item();
            item2.Level = 50;
            var item3 = new Item();
            item3.Level = 20;
            playerExtensionTest.Items.Add(item1);
            playerExtensionTest.Items.Add(item2);
            playerExtensionTest.Items.Add(item3);
            Console.WriteLine("Highest level: " + playerExtensionTest.GetHighestLevelItem().Level);

            // 3.
            Console.WriteLine("3.");
            var array1 = GetItems(playerExtensionTest);
            var array2 = GetItemsWithLinq(playerExtensionTest);

            foreach(var item in array1)
            {
                Console.WriteLine(item.Level);
            }
            foreach (var item in array2)
            {
                Console.WriteLine(item.Level);
            }

            // 4.
            Console.WriteLine("4.");
            Console.WriteLine(FirstItem(playerExtensionTest));
            Console.WriteLine(FirstItemWithLinq(playerExtensionTest));

            // 5.
            Console.WriteLine("5.");
            playerExtensionTest.Items[0].Id = Guid.NewGuid();
            playerExtensionTest.Items[1].Id = Guid.NewGuid();
            playerExtensionTest.Items[2].Id = Guid.NewGuid();
            ProcessEachItem(playerExtensionTest, PrintItem);

            // 6.
            Console.WriteLine("6.");
            Action<Player> lambdaFunction = (player) => ProcessEachItem(player, PrintItem);
            lambdaFunction(playerExtensionTest);

            // 7.
            Console.WriteLine("7.");

            var rand = new Random();
            var playerTestList = new List<Player>();
            for (int i = 0; i < 20; i++)
            {
                var playerTest = new Player();
                playerTest.Score = rand.Next(1000);
                playerTestList.Add(playerTest);
            }
            var game1 = new Game<Player>(playerTestList);

            var playerTestList2 = new List<PlayerForAnotherGame>();
            for (int i = 0; i < 20; i++)
            {
                var playerTest = new PlayerForAnotherGame();
                playerTest.Score = rand.Next(1000);
                playerTestList2.Add(playerTest);
            }
            var game2 = new Game<PlayerForAnotherGame>(playerTestList2);

            foreach(var p in game1.GetTop10Players())
            {
                Console.WriteLine(p.Score);
            }
            Console.WriteLine("---");
            foreach (var p in game2.GetTop10Players())
            {
                Console.WriteLine(p.Score);
            }
        }

        public static Item[] GetItems(Player player)
        {
            int length = player.Items.Count;
            Item[] items = new Item[length];

            for(int i = 0; i < length; i++)
            {
                items[i] = player.Items[i];
            }
            return items;
        }

        public static Item[] GetItemsWithLinq(Player player)
        {
            return player.Items.ToArray();
        }

        public static Item FirstItem(Player player)
        {
            if (player.Items == null)
                return null;
            return player.Items[0];
        }

        public static Item FirstItemWithLinq(Player player)
        {
            return player.Items.FirstOrDefault<Item>();
        }

        public static void ProcessEachItem(Player player, Action<Item> process)
        {
            foreach (Item item in player.Items)
            {
                process(item);
            }
        }

        public static void PrintItem(Item item)
        {
            Console.WriteLine("Id: " + item.Id + " level: " + item.Level);
        }
    }

    public class Game<T> where T : IPlayer
    {
        private List<T> _players;

        public Game(List<T> players)
        {
            _players = players;
        }

        public T[] GetTop10Players()
        {
            //sort
            for (int j = 0; j < _players.Count - 1; j++)
            {
                for (int i = 0;  i < _players.Count -1; i++)
                {
                    if(_players[i].Score < _players[i+1].Score)
                    {
                        int tmp = _players[i + 1].Score;
                        _players[i + 1].Score = _players[i].Score;
                        _players[i].Score = tmp;
                    }
                }
            }

            T[] returnArray = new T[10];
            for(int i = 0; i < 10; i++)
            {
                returnArray[i] = _players[i];
            }
            return returnArray;
        }
    }

    public class PlayerForAnotherGame : IPlayer
    {
        public int Score { get; set; }
    }
}
