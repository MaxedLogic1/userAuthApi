using Microsoft.EntityFrameworkCore;
using usersAuthApi.ApplicationDbContext;
using usersAuthApi.Models.DTO;
using System.Threading.Tasks;

namespace usersAuthApi.Repositories
{
    public class PlayerAndGameRepository : IPlayerAndGameRepository
    {
        private readonly userDbContext _userDbContext;

        public PlayerAndGameRepository(userDbContext userDbContext)
        {
            _userDbContext = userDbContext;
        }

        public async Task<bool> GetPlayerAndGameAsync(int PId, int GId)
        {
            // Check if the player exists
            var player = await _userDbContext.Tab_Register
                .Where(u => u.Id == PId)
                .FirstOrDefaultAsync();

            // If the player doesn't exist, return false
            if (player == null)
            {
                return false;
            }

            // Check if the game exists
            var game = await _userDbContext.Tab_Games
                .Where(g => g.Id == GId)
                .FirstOrDefaultAsync();

            // If the game doesn't exist, return false
            if (game == null)
            {
                return false;
            }

            // If both player and game exist, return true
            return true;
        }

        public async Task<string> GetPlayerName(int Pid)
        {
            //player is exsit
            var player = await _userDbContext.Tab_Register
              .Where(u => u.Id == Pid)
              .FirstOrDefaultAsync();
            if (player == null)
            {
                return null;
            }
            else
            {
                return player.Name;
            }
        }
    }
}
