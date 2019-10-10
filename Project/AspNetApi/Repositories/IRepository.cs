using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetApi.Models;

namespace AspNetApi.Repositories
{
    public interface IRepository
    {
        Task<PlayerInformation> GetPlayerInformation(Guid playerId);
        Task<PlayerInformationArrayHolder> GetAllPlayers();
        Task<PlayerInformationArrayHolder> GetAllPlayers(string name);
        Task<PlayerInformationArrayHolder> GetHighestScoringPlayers(string filterDate, string filterLevel, int amountOfScores);
        Task<PlayerInformation> CreatePlayer(Player player);
        Task<PlayerInformation> ModifyPlayer(Guid playerId, ModifiedPlayer player);
        Task<PlayerInformation> DeletePlayer(Guid playerId, ModifiedPlayer playerThatDeletes);

        Task<Run> GetRun(Guid playerId, Guid runId);
        Task<Run[]> GetAllRuns(Guid playerId); 
        Task<Run> CreateRun(Guid playerId, Run run);
        Task<Run> ModifyRun(Guid playerId, Guid runId, ModifiedRun run);
        Task<Run> DeleteRun(Guid playerId, Guid runId, ModifiedPlayer playerThatDeletes);
        Task<RunArrayHolder> GetHighestScore(Guid playerId, string filterDate, string filterLevel);

        Task<SaveData> Load(Guid playerId, NewPlayer player);
        Task<SaveData> Save(Guid playerId, SaveData saveData);
        Task<SaveData> DeleteSave(Guid playerId, NewPlayer player);
    }
}
