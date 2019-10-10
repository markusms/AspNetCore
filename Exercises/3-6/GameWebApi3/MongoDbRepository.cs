using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameWebApi3
{
    public class MongoDbRepository : IRepository
    {
        private readonly IMongoCollection<Player> collection;
        private readonly IMongoCollection<BsonDocument> bsonDocumentCollection;

        public MongoDbRepository()
        {
            var mongoClient = new MongoClient("mongodb://localhost:27017");
            var database = mongoClient.GetDatabase("game");
            collection = database.GetCollection<Player>("players");
            bsonDocumentCollection = database.GetCollection<BsonDocument>("players");
        }

        public async Task<Player> Create(Player player)
        {
            await collection.InsertOneAsync(player);
            return player;
        }

        public Task<Player> Get(Guid id)
        {
            var filter = Builders<Player>.Filter.Eq(p => p.Id, id);
            var player = collection.Find(filter).FirstAsync();
            if (player == null)
                throw new NotFoundException();
            return player;
        }

        public Task<Player> Get(string name)
        {
            var filter = Builders<Player>.Filter.Eq(p => p.Name, name);
            var player = collection.Find(filter).FirstAsync();
            if (player == null)
                throw new NotFoundException();
            return player;
        }

        public Task<Player> GetQuery(string name)
        {
            var filter = Builders<Player>.Filter.Eq(p => p.Name, name);
            var player = collection.Find(filter).FirstAsync();
            if (player == null)
                throw new NotFoundException();
            return player;
        }

        public async Task<Player[]> GetAll()
        {
            var players = await collection.Find(new BsonDocument()).ToListAsync();
            return players.ToArray();
        }

        public async Task<Player[]> GetAll(int minScore)
        {
            if (minScore != 0)
            {
                FilterDefinition<Player> filter = Builders<Player>.Filter.Gt("Score", minScore);
                var players = await collection.Find(filter).ToListAsync();
                return players.ToArray();
            }
            else
            {
                var players = await collection.Find(new BsonDocument()).ToListAsync();
                return players.ToArray();
            }
        }

        public async Task<Player[]> GetAll(string name)
        {
            FilterDefinition<Player> filter = Builders<Player>.Filter.Eq(p => p.Name, name);
            var player = await collection.Find(filter).ToListAsync();
            if (player == null)
                throw new NotFoundException();
            return player.ToArray();
        }

        public async Task<Player[]> GetAllQuery(int minScore)
        {
            FilterDefinition<Player> filter = Builders<Player>.Filter.Gt("Score", minScore);
            List<Player> players = await collection.Find(filter).ToListAsync();
            return players.ToArray();
        }

        public async Task<Player> Modify(Guid id, ModifiedPlayer player)
        {
            Player newPlayer = await Get(id);
            newPlayer.Score = player.Score;
            FilterDefinition<Player> filter = Builders<Player>.Filter.Eq(p => p.Id, newPlayer.Id);
            await collection.ReplaceOneAsync(filter, newPlayer);
            return newPlayer;
        }

        public async Task<Player> ModifyWithoutFetching(Guid id, ModifiedPlayer player)
        {
            FilterDefinition<Player> filter = Builders<Player>.Filter.Eq(p => p.Id, id);
            var update = Builders<Player>.Update.Set(p => p.Score, player.Score);
            var result = await collection.UpdateOneAsync(filter, update);
            //fetching here for the return but not for update
            Player newPlayer = await Get(id);
            return newPlayer;
        }

        public async Task<Player> IncrementScoreWithoutFetching(Guid id)
        {
            FilterDefinition<Player> filter = Builders<Player>.Filter.Eq(p => p.Id, id);
            var update = Builders<Player>.Update.Inc(p => p.Score, 1);
            var result = await collection.UpdateOneAsync(filter, update);
            //fetching here for the return but not for update
            Player newPlayer = await Get(id);
            return newPlayer;
        }

        public async Task<Player> PushItem(Guid playerId, Item item)
        {
            FilterDefinition<Player> filter = Builders<Player>.Filter.Eq(p => p.Id, playerId);
            var update = Builders<Player>.Update.Push(p => p.Items, item);
            var result = await collection.UpdateOneAsync(filter, update);
            //fetching here for the return but not for update
            Player newPlayer = await Get(playerId);
            return newPlayer;
        }

        public async Task<Player> Delete(Guid playerId)
        {
            FilterDefinition<Player> filter = Builders<Player>.Filter.Eq(p => p.Id, playerId);
            return await collection.FindOneAndDeleteAsync(filter);
        }

        public async Task<Player[]> GetBetweenLevelsAsync(int minLevel, int maxLevel)
        {
            var filter = Builders<Player>.Filter.Gte(p => p.Level, 18) & Builders<Player>.Filter.Lte(p => p.Level, 30);
            var players = await collection.Find(filter).ToListAsync();
            return players.ToArray();
        }

        public async Task<Player> IncreasePlayerScoreAndRemoveItem(Guid playerId, Guid itemId, int score)
        {
            var pull = Builders<Player>.Update.PullFilter(p => p.Items, i => i.Id == itemId);
            var inc = Builders<Player>.Update.Inc(p => p.Score, score);
            var update = Builders<Player>.Update.Combine(pull, inc);
            var filter = Builders<Player>.Filter.Eq(p => p.Id, playerId);

            return await collection.FindOneAndUpdateAsync(filter, update);
        }

        public async Task<int> GetCommonLevel()
        {
            Dictionary<int, int> commonLevel = new Dictionary<int, int>();
            var players = await GetAll(0);
            foreach (var player in players)
            {
                var level = player.Level;
                if (commonLevel.ContainsKey(level))
                {
                    commonLevel[level]++;
                }
                else
                {
                    commonLevel[level] = 1;
                }
            }

            int largest = 0;
            foreach (var amount in commonLevel.Values)
            {
                if (amount > largest)
                    largest = amount;
            }

            return largest;
        }

        public async Task<Player[]> GetByTag(Tags tag)
        {
            FilterDefinition<Player> filter = Builders<Player>.Filter.Eq(p => p.Tags, tag);
            var player = await collection.Find(filter).ToListAsync();
            if (player == null)
                throw new NotFoundException();
            return player.ToArray();
        }

        public async Task<Player[]> GetPlayersWithItemsOfLevel(int itemLevel)
        {
            var playersWithItemsOfLevel =
                Builders<Player>.Filter.ElemMatch<Item>(
                    p => p.Items,
                    Builders<Item>.Filter.Eq(
                        i => i.Level,
                        itemLevel));
            var player = await collection.Find(playersWithItemsOfLevel).ToListAsync();
            if (player == null)
                throw new NotFoundException();
            return player.ToArray();
        }

        public async Task<Player[]> GetPlayersWithAmountOfItems(int amountOfItems)
        {
            FilterDefinition<Player> filter = Builders<Player>.Filter.Size(p => p.Items, amountOfItems);
            var players = await collection.Find(filter).ToListAsync();
            if (players == null)
                throw new NotFoundException();
            return players.ToArray();
        }

        public async Task<Player[]> GetHighestScoringPlayers()
        {
            var players = await collection.Find(FilterDefinition<Player>.Empty).Limit(10).Sort("{Score: -1}").ToListAsync();
            return players.ToArray();
        }

        public async Task<Item[]> GetCountForItemsOfLevel()
        {
            var allItems = await collection.Aggregate()
                .Unwind(p => p.Items)
                .As<Item>()
                .ToListAsync();

            return allItems.ToArray();
        }

        public async Task<Item> CreateItem(Guid playerId, Item item)
        {
            Player player = await Get(playerId);
            Item[] newItems = new Item[player.Items.Count() + 1];
            for(int i = 0; i < player.Items.Count(); i++)
            {
                newItems[i] = player.Items[i];
            }
            newItems[newItems.Count() - 1] = item;
            player.Items = newItems;
            FilterDefinition<Player> filter = Builders<Player>.Filter.Eq(p => p.Id, player.Id);
            await collection.ReplaceOneAsync(filter, player);
            return item;
        }

        public async Task<Item> GetItem(Guid playerId, Guid itemId)
        {
            Player player = await Get(playerId);
            foreach(var item in player.Items)
            {
                if(item.Id == itemId)
                {
                    return item;
                }
            }
            throw new NotFoundException();
        }
        public async Task<Item[]> GetAllItems(Guid playerId)
        {
            Player player = await Get(playerId);
            if (player != null)
                return player.Items;
            throw new NotFoundException();
        }

        public async Task<Item> UpdateItem(Guid playerId, Guid itemId, ModifiedItem modifiedItem)
        {
            Player player = await Get(playerId);
            foreach (var item in player.Items)
            {
                if (item.Id == itemId)
                {
                    item.Level = modifiedItem.Level;
                    FilterDefinition<Player> filter = Builders<Player>.Filter.Eq(p => p.Id, player.Id);
                    await collection.ReplaceOneAsync(filter, player);
                    return item;
                }
            }
            throw new NotFoundException();
        }
        public async Task<Item> DeleteItem(Guid playerId, Guid itemId)
        {
            Player player = await Get(playerId);

            foreach (var item in player.Items.Reverse<Item>())
            {
                if(item.Id == itemId)
                {
                    var newItems = player.Items.Where(val => val != item).ToArray(); 
                    player.Items = newItems; //remove item from the array
                    FilterDefinition<Player> filter = Builders<Player>.Filter.Eq(p => p.Id, playerId);
                    await collection.ReplaceOneAsync(filter, player);
                    return item;
                }
            }

            throw new NotFoundException();
        }

    }
}
