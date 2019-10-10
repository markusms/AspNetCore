using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameWebApi3
{
    [Route("api/players")]
    [ApiController]
    public class PlayersController : ControllerBase
    {
        IRepository repository;

        public PlayersController(IRepository repository)
        {
            this.repository = repository;
        }

        [HttpGet]
        [Route("{playerId}")]
        public async Task<Player> Get(Guid playerId)
        {
            return await repository.Get(playerId);
        }

        [HttpGet]
        [Route("")]
        public async Task<Player[]> GetAll([FromQuery(Name = "name")] string name, [FromQuery(Name = "minScore")] int minScore = 0)
        {
            if(name == null && minScore == 0)
                return await repository.GetAll();
            else if(minScore != 0)
                return await repository.GetAll(minScore);
            else
                return await repository.GetAll(name);
        }

        [HttpGet]
        [Route("commonlevel")]
        public async Task<int> GetCommonLevel()
        {
            return await repository.GetCommonLevel();
        }

        [HttpGet]
        [Route("tags/{tag}")]
        public async Task<Player[]> GetByTag(string tag)
        {
            Enum.TryParse(tag, out Tags parsedTag);
            return await repository.GetByTag(parsedTag);
        }

        [HttpGet]
        [Route("itemLevel/{level}")]
        public async Task<Player[]> GetPlayersWithItemsOfLevel(int level)
        {
            return await repository.GetPlayersWithItemsOfLevel(level);
        }

        [HttpGet]
        [Route("amountOfItems/{amountOfItems}")]
        public async Task<Player[]> GetPlayersWithAmountOfItems(int amountOfItems)
        {
            return await repository.GetPlayersWithAmountOfItems(amountOfItems);
        }

        [HttpGet]
        [Route("top10")]
        public async Task<Player[]> GetHighestScoringPlayers()
        {
            return await repository.GetHighestScoringPlayers();
        }

        [HttpGet]
        [Route("allItems")]
        public async Task<Item[]> GetCountForItemsOfLevel()
        {
            return await repository.GetCountForItemsOfLevel();
        }

        [HttpPost]
        [Route("")]
        public async Task<Player> Create(NewPlayer player)
        {
            var newPlayer = new Player(player);
            newPlayer.Level = 25;
            newPlayer.Tags = Tags.active;
            await repository.Create(newPlayer);
            return newPlayer;
        }

        [HttpPut]
        [Route("{playerId}")]
        public async Task<Player> Modify(Guid playerId, ModifiedPlayer player)
        {
            return await repository.Modify(playerId, player);
        }

        [HttpPut]
        [Route("modify/{playerId}")]
        public async Task<Player> ModifyWithoutFetching(Guid playerId, ModifiedPlayer player)
        {
            return await repository.ModifyWithoutFetching(playerId, player);
        }

        [HttpPut]
        [Route("incrementScore/{playerId}")]
        public async Task<Player> IncrementScoreWithoutFetching(Guid playerId, ModifiedPlayer player)
        {
            return await repository.IncrementScoreWithoutFetching(playerId);
        }

        [HttpPut]
        [Route("pushItem/{playerId}")]
        public async Task<Player> IncrementScoreWithoutFetching(Guid playerId, NewItem newItem)
        {
            Item item = new Item(newItem);
            return await repository.PushItem(playerId, item);
        }


        [HttpDelete]
        [Route("{playerId}")]
        public async Task<Player> Delete(Guid playerId)
        {
            return await repository.Delete(playerId);
        }
    }
}
