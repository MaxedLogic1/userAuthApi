using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using usersAuthApi.ApplicationDbContext;
using usersAuthApi.Exceptions;
using usersAuthApi.Models.DTO;

namespace usersAuthApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AllGameHistoryController : ControllerBase
    {
        private readonly userDbContext _userDbContext;

        public AllGameHistoryController(userDbContext userDbContext) 
        {
            _userDbContext = userDbContext;
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<List<AllGameIndexRequestDto>> GetPlayerGameHistory([FromRoute] int id)
        {
            try
            {
                var playerId = id;

                if (playerId <= 0)
                {
                    throw new PlayerNotFoundCustomException();
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
                        .OrderBy(game => game.EntryDate)
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
                        .OrderBy(game => game.EntryDate)
                         .ToListAsync();
                // Fetching Bubble games for the player
                var flappyBird = await _userDbContext.Tab_FlappyBirdGameIndex
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
                       .OrderBy(game => game.EntryDate)
                        .ToListAsync();
                // Combine both results
                var allGames = headTailGames.Concat(bubbleGames).Concat(flappyBird).ToList();

                // If no games are found, throw custom exception
                if (allGames == null || allGames.Count == 0)
                {
                    throw new ArgumentException("Game and Player not found.");

                }

                return allGames;
            }

            catch (PlayerNotFoundCustomException)
            {
                throw new PlayerNotFoundCustomException("Player ID is invalid or not found.");
            }
            catch (GameNotFoundCustomException)
            {
                throw new GameNotFoundCustomException("Game ID is invalid or not found.");
            }
        }


    }
}
