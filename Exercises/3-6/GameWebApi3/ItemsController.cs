using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameWebApi3
{
    [Route("api/players/{playerId}/items")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        IRepository repository;

        public ItemsController(IRepository repository)
        {
            this.repository = repository;
        }

        [HttpGet]
        [Route("{itemsId}")]
        public async Task<Item> Get(Guid playerId, Guid itemsId)
        {
            return await repository.GetItem(playerId, itemsId);
        }

        [HttpGet]
        [Route("")]
        public async Task<Item[]> GetAll(Guid playerId)
        {
            return await repository.GetAllItems(playerId);
        }

        [TooLowLevelException]
        [HttpPost]
        [Route("")]
        public async Task<Item> Create(Guid playerId, NewItem item)
        {
            item.ItemType = ItemType.SWORD;
            item.Level = 5;
            var newItem = new Item(item);
            await repository.CreateItem(playerId, newItem);
            return newItem;
        }

        [HttpPut]
        [Route("{itemsId}")]
        public async Task<Item> Modify(Guid playerId, Guid itemsId, ModifiedItem item)
        {
            return await repository.UpdateItem(playerId, itemsId, item);
        }

        [HttpDelete]
        [Route("{itemsId}")]
        public async Task<Item> Delete(Guid playerId, Guid itemsId)
        {
            return await repository.DeleteItem(playerId, itemsId);
        }

        [HttpDelete]
        [Route("sell/{itemsId}")]
        public async Task<Player> IncreasePlayerScoreAndRemoveItem(Guid playerId, Guid itemsId)
        {
            return await repository.IncreasePlayerScoreAndRemoveItem(playerId, itemsId, 10);
        }
    }
}
