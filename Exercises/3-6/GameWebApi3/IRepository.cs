using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameWebApi3
{
    public interface IRepository
    {
        Task<Player> Get(Guid id);
        Task<Player> Get(string name);
        Task<Player[]> GetAll();
        Task<Player[]> GetAll(string name); //2. Selector matching
        Task<Player[]> GetAll(int minScore); //1. Ranges
        Task<Player> Create(Player player);
        Task<Player> Modify(Guid id, ModifiedPlayer player);
        Task<Player> ModifyWithoutFetching(Guid id, ModifiedPlayer player); //6. Update
        Task<Player> IncrementScoreWithoutFetching(Guid id); //7. Increment
        Task<Player> PushItem(Guid id, Item item); //8. Push
        Task<Player> Delete(Guid id);
        Task<int> GetCommonLevel(); //11. Aggregation
        Task<Player[]> GetByTag(Tags tag); //tags 3. Set operators
        Task<Player[]> GetPlayersWithItemsOfLevel(int itemLevel); //4. Sub documents queries
        Task<Player[]> GetPlayersWithAmountOfItems(int amountOfItems); //5. Size
        Task<Player[]> GetHighestScoringPlayers(); //10. Sorting
        Task<Player> IncreasePlayerScoreAndRemoveItem(Guid playerId, Guid itemId, int score); //9. Pop and increment as an atomic operation

        Task<Item> CreateItem(Guid playerId, Item item);
        Task<Item> GetItem(Guid playerId, Guid itemId);
        Task<Item[]> GetAllItems(Guid playerId);
        Task<Item> UpdateItem(Guid playerId, Guid itemsId, ModifiedItem item);
        Task<Item> DeleteItem(Guid playerId, Guid itemId);
        Task<Item[]> GetCountForItemsOfLevel();
    }
}
