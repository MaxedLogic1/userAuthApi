using usersAuthApi.Models.DTO;

namespace usersAuthApi.Repositories
{
    public interface IPlayerAndGameRepository
    {
        Task<bool> GetPlayerAndGameAsync(int PId, int GId);
        Task<string> GetPlayerName(int Pid);
    }
}
