using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace GameWebApi3
{
    public class FileRepository : IRepository
    {
        public const string fileNameDev = "game-dev.txt";
        public const string fileNameProd = "game-prod.txt";
        public readonly string filePath = Path.Combine(Environment.CurrentDirectory, @"Data\", fileNameDev);

        public FileRepository()
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            if(environment == Microsoft.Extensions.Hosting.Environments.Production)
            {
                filePath = Path.Combine(Environment.CurrentDirectory, @"Data\", fileNameProd);
            }
            else if (environment == Microsoft.Extensions.Hosting.Environments.Development)
            {
                filePath = Path.Combine(Environment.CurrentDirectory, @"Data\", fileNameDev);
            }

        }

        public Task<Player> Create(Player player)
        {
            var getPlayers = GetPlayers();
            getPlayers.Wait();
            Player[] playerList = getPlayers.Result;
            int length = playerList.Length;
            PlayerList newPlayerList = new PlayerList();
            newPlayerList.Players = new Player[length + 1];
            for (int i = 0; i < length; i++)
            {
                newPlayerList.Players[i] = playerList[i];
            }
            newPlayerList.Players[length] = player;
            File.WriteAllText(filePath, JsonConvert.SerializeObject(newPlayerList));
            return Task.FromResult(player);
        }

        public Task<Player> Delete(Guid id)
        {
            int found = -1;
            var getPlayers = GetPlayers();
            getPlayers.Wait();
            Player[] playerList = getPlayers.Result;
            int length = playerList.Length;
            for (int i = 0; i < length; i++)
            {
                if (playerList[i].Id == id)
                {
                    found = i;
                    break;
                }
            }

            PlayerList newPlayerList = new PlayerList();
            newPlayerList.Players = new Player[length - 1];
            if (found != -1)
            {
                for (int i = 0; i < length - 1; i++)
                {
                    if (i < found)
                    {
                        newPlayerList.Players[i] = playerList[i];
                    }
                    else
                    {
                        newPlayerList.Players[i] = playerList[i + 1];
                    }
                }
                File.WriteAllText(filePath, JsonConvert.SerializeObject(newPlayerList));
                return Task.FromResult(playerList[found]);
            }
            else
            {
                //return Task.FromException<Player>(new Exception("guid not found"));
                throw new NotFoundException();
            }
        }

        public Task<Player> Get(Guid id)
        {
            var getPlayers = GetPlayers();
            getPlayers.Wait();
            Player[] playerList = getPlayers.Result;
            foreach (var player in playerList)
            {
                if (player.Id == id)
                {
                    return Task.FromResult(player);
                }
            }
            //return Task.FromException<Player>(new Exception("guid not found"));
            throw new NotFoundException();
        }

        public Task<Player[]> GetAll(int minScore)
        {
            return GetPlayers();
        }

        public Task<Player> Modify(Guid id, ModifiedPlayer player)
        {
            int found = -1;
            var getPlayers = GetPlayers();
            getPlayers.Wait();
            Player[] playerList = getPlayers.Result;
            int length = playerList.Length;
            for (int i = 0; i < length; i++)
            {
                if (playerList[i].Id == id)
                {
                    found = i;
                }
            }

            if (found != -1)
            {
                playerList[found].Score = player.Score;
                File.WriteAllText(filePath, JsonConvert.SerializeObject(playerList));
                return Task.FromResult(playerList[found]);
            }
            else
            {
                //not found
                //return Task.FromException<Player>(new Exception("guid not found"));
                throw new NotFoundException();
            }
        }

        private Task<Player[]> GetPlayers()
        {
            try
            {
                string json = File.ReadAllText(filePath);
                var playerList = JsonConvert.DeserializeObject<PlayerList>(json);
                return Task.FromResult(playerList.Players);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error reading the file: " + e);
                throw e;
            }
        }

        public Task<Item> CreateItem(Guid playerId, Item item)
        {
            bool found = false;
            var getPlayers = GetPlayers();
            getPlayers.Wait();
            Player[] playerList = getPlayers.Result;
            foreach (var player in playerList)
            {
                if (player.Id == playerId)
                {
                    if(item.ItemType == ItemType.SWORD && player.Level < 3)
                    {
                        throw new TooLowLevelException();
                    }

                    found = true;
                    Item[] newItems = new Item[player.Items.Count() + 1];
                    newItems[newItems.Count()-1] = item;
                    player.Items = newItems;
                }
            }
            if (!found) //not found
                throw new NotFoundException();

            PlayerList newPlayerList = new PlayerList();
            newPlayerList.Players = playerList;
            File.WriteAllText(filePath, JsonConvert.SerializeObject(newPlayerList));
            return Task.FromResult(item);
        }

        public Task<Item> DeleteItem(Guid playerId, Guid itemsId)
        {
            Item itemFound = null;
            int found = -1;
            var getPlayers = GetPlayers();
            getPlayers.Wait();
            Player[] playerList = getPlayers.Result;
            int length = playerList.Length;
            for (int i = 0; i < length; i++)
            {
                if (playerList[i].Id == playerId)
                {
                    foreach (var item in playerList[i].Items.Reverse<Item>())
                    {
                        itemFound = item;
                        playerList[i].Items.Where(val => val != item).ToArray(); //remove item from the array
                        found = i;
                        break;
                    }
                    break;
                }
            }

            if (found == -1) //did not find player/item
            {
                //return Task.FromException<Item>(new Exception("guid not found"));
                throw new NotFoundException();
            }
            else
            {
                PlayerList newPlayerList = new PlayerList();
                newPlayerList.Players = playerList;
                File.WriteAllText(filePath, JsonConvert.SerializeObject(newPlayerList));
            }
            return Task.FromResult(itemFound);
        }

        public Task<Item> GetItem(Guid playerId, Guid itemId)
        {
            var getPlayers = GetPlayers();
            getPlayers.Wait();
            Player[] playerList = getPlayers.Result;
            foreach (var player in playerList)
            {
                if (player.Id == playerId)
                {
                    foreach (var item in player.Items)
                    {
                        if (item.Id == itemId)
                            return Task.FromResult(item);
                    }
                }
            }
            //return Task.FromException<Item>(new Exception("not found"));
            throw new NotFoundException();
        }

        public Task<Item[]> GetAllItems(Guid playerId)
        {
            var getPlayers = GetPlayers();
            getPlayers.Wait();
            Player[] playerList = getPlayers.Result;
            foreach (var player in playerList)
            {
                if (player.Id == playerId)
                {
                    return Task.FromResult(player.Items.ToArray());
                }
            }
            //return Task.FromException<Item[]>(new Exception("not found"));
            throw new NotFoundException();
        }

        public Task<Item> UpdateItem(Guid playerId, Guid itemsId, ModifiedItem modifiedItem)
        {
            int found = -1;
            var getPlayers = GetPlayers();
            getPlayers.Wait();
            Player[] playerList = getPlayers.Result;
            int length = playerList.Length;
            for (int i = 0; i < length; i++)
            {
                if (playerList[i].Id == playerId)
                {
                    foreach (var item in playerList[i].Items)
                    {
                        if (item.Id == itemsId)
                        {
                            found = i;
                            item.Level = modifiedItem.Level;
                            PlayerList newPlayerList = new PlayerList();
                            newPlayerList.Players = playerList;
                            File.WriteAllText(filePath, JsonConvert.SerializeObject(newPlayerList));
                            return Task.FromResult(item);
                        }
                    }
                }
            }
            //return Task.FromException<Item>(new Exception("not found"));
            throw new NotFoundException();
        }

        public Task<int> GetCommonLevel()
        {
            throw new NotImplementedException();
        }

        public Task<Player> Get(string name)
        {
            throw new NotImplementedException();
        }

        public Task<Player> GetQuery(int minScore, string name)
        {
            throw new NotImplementedException();
        }

        public Task<Player[]> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<Player[]> GetAll(string name)
        {
            throw new NotImplementedException();
        }

        public Task<Player[]> GetByTag(Tags tag)
        {
            throw new NotImplementedException();
        }

        public Task<Player[]> GetPlayersWithItemsOfLevel(int itemLevel)
        {
            throw new NotImplementedException();
        }

        public Task<Player[]> GetPlayersWithAmountOfItems(int amountOfItems)
        {
            throw new NotImplementedException();
        }

        public Task<Player> ModifyWithoutFetching(Guid id, ModifiedPlayer player)
        {
            throw new NotImplementedException();
        }

        public Task<Player> IncrementScoreWithoutFetching(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<Player> PushItem(Guid id, Item item)
        {
            throw new NotImplementedException();
        }

        public Task<Player> IncreasePlayerScoreAndRemoveItem(Guid playerId, Guid itemId, int score)
        {
            throw new NotImplementedException();
        }

        public Task<Player[]> GetHighestScoringPlayers()
        {
            throw new NotImplementedException();
        }

        public Task<Item[]> GetCountForItemsOfLevel()
        {
            throw new NotImplementedException();
        }
    }
}
