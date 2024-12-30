using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using usersAuthApi.ApplicationDbContext;
using usersAuthApi.Models.DTO;

namespace usersAuthApi.Controllers
{
    [Route("Api/[controller]")]
    [ApiController]
    public class AllGameIndexController : ControllerBase
    {
        private readonly userDbContext _userDbContext;

        public AllGameIndexController(userDbContext userDbContext)
        {
            _userDbContext = userDbContext;
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<List<AllGameIndexRequestDto>> GetAllGamesForPlayer([FromRoute] int id)
        {
            try
            {
                var playerId = id;

                if (playerId <= 0)
                {
                    throw new InvalidOperationException("Player ID is invalid or not found.");
                }

                // Fetching HeadTail games for the player
                var headTailGames = await _userDbContext.Tab_HeadTailGameIndex
                    .Where(h => h.PId == playerId)
                    .Join(
                        _userDbContext.Tab_Games,
                        h => h.GId,
                        g => g.Id,
                        (h, g) => new AllGameIndexRequestDto
                        {
                            GameName = g.Name,
                            BetAmount = h.BetAmount,
                            EntryDate = h.EntryDate,
                            WinLoss = h.Type
                        })
                    .ToListAsync();

                // Fetching Bubble games for the player
                var bubbleGames = await _userDbContext.Tab_BubbleGameIndex
                    .Where(b => b.PId == playerId)
                    .Join(
                        _userDbContext.Tab_Games,
                        b => b.GId,
                        g => g.Id,
                        (b, g) => new AllGameIndexRequestDto
                        {
                            GameName = g.Name,
                            BetAmount = b.BetAmount,
                            EntryDate = b.EntryDate,
                            WinLoss = b.Type
                        })
                    .ToListAsync();

                // Combine both results
                var allGames = headTailGames.Concat(bubbleGames).ToList();

                // If no games are found, throw custom exception
                if (allGames == null || allGames.Count == 0)
                {
                    throw new InvalidOperationException("Game and Player not found.");

                }

                return allGames;
            }
            
            catch (Exception)
            {
                throw new InvalidOperationException("An unexpected error occurred while fetching games.");
            }
        }


    }
}
