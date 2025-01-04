using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using usersAuthApi.ApplicationDbContext;
using usersAuthApi.Exceptions;
using usersAuthApi.Models.DTO;

namespace usersAuthApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class FlappyBirdGameIndexController
    {
        private readonly userDbContext _userDbContext;

        public FlappyBirdGameIndexController(userDbContext userDbContext)
        {
            _userDbContext = userDbContext;
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<List<FlappyBirdGameIndexResponseDto>> GetFlappyBirdGameIndex([FromRoute] int id)
        {
            if (id <= 0)
            {
                throw new PlayerNotFoundCustomException("Player ID is invalid or not found.");
            }

            try
            {
                var player = await _userDbContext.Tab_Register
                    .Where(u => u.Id == id)
                    .FirstOrDefaultAsync();

                if (player == null)
                {
                    throw new PlayerNotFoundCustomException("Player not found.");
                }

                var flappyBirdGames = await _userDbContext.Tab_FlappyBirdGameIndex
                    .Where(b => b.PId == id)
                    .Join(
                        _userDbContext.Tab_Games,
                        b => b.GId,
                        g => g.Id,
                        (b, g) => new FlappyBirdGameIndexResponseDto
                        {
                            GameName = g.Name,
                            BetAmount = b.BetAmount,
                            EntryDate = b.EntryDate,
                            Type = b.Type
                        })
                    .OrderBy(game => game.EntryDate) // Order by EntryDate descending
                    .ToListAsync();

                if (flappyBirdGames == null || !flappyBirdGames.Any())
                {
                    throw new GameNotFoundCustomException("No result found. The player has not played This game.");
                }

                return flappyBirdGames;
            }
            catch (PlayerNotFoundCustomException ex)
            {
                throw ex; // Player not found exception
            }
            catch (GameNotFoundCustomException ex)
            {
                throw ex; // Game not found exception (player has not played)
            }
            catch (Exception ex)
            {
                // Catch any unexpected errors
                throw new Exception("An unexpected error occurred while processing the game data.", ex);
            }
        }

    }

}

