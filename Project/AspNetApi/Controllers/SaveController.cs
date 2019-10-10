using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetApi.Models;
using AspNetApi.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AspNetApi.Controllers
{
    [Route("api/players/{playerId}/save")]
    [ApiController]
    public class SaveController : ControllerBase
    {
        private IRepository repository;

        public SaveController(IRepository repository)
        {
            this.repository = repository;
        }
        [HttpGet] 
        [Route("")]
        public async Task<SaveData> Get(Guid playerId, NewPlayer player)
        {
            return await repository.Load(playerId, player);
        }

        [HttpPut] //Has to be put because the game's unity framework doesn't support GET or DELETE with body.
        [Route("")]
        public async Task<SaveData> Unity(Guid playerId, NewPlayer player, [FromQuery(Name = "delete")] string delete)
        {
            if(delete == "delete")
                return await repository.DeleteSave(playerId, player);
            else
                return await repository.Load(playerId, player);
        }

        [HttpPost]
        [Route("")]
        public async Task<SaveData> Create(Guid playerId, SaveData saveData)
        {
            return await repository.Save(playerId, saveData);
        }

        [HttpDelete]
        [Route("")]
        public async Task<SaveData> Delete(Guid playerId, NewPlayer player)
        {
            return await repository.DeleteSave(playerId, player);
        }
    }
}
