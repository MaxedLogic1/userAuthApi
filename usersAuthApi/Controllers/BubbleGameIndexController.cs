using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SendGrid.Helpers.Mail;
using usersAuthApi.ApplicationDbContext;
using usersAuthApi.Exceptions;
using usersAuthApi.Models.DTO;

namespace usersAuthApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class BubbleGameIndexController : ControllerBase
    {
        private readonly userDbContext _userDbContext;

        public BubbleGameIndexController(userDbContext userDbContext)
        {
            _userDbContext = userDbContext;
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<List<BubbleGameIndexResponseDto>> GetBubbleGameIndex([FromRoute] int id)
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

                var bubbleGames = await _userDbContext.Tab_BubbleGameIndex
                    .Where(b => b.PId == id)
                    .Join(
                        _userDbContext.Tab_Games,
                        b => b.GId,
                        g => g.Id,
                        (b, g) => new BubbleGameIndexResponseDto
                        {
                            GameName = g.Name,
                            BetAmount = b.BetAmount,
                            EntryDate = b.EntryDate,
                            Type = b.Type
                        })
                    .OrderBy(game => game.EntryDate) // Order by EntryDate descending
                    .ToListAsync();

                if (bubbleGames == null || !bubbleGames.Any())
                {
                    throw new GameNotFoundCustomException("No result found. The player has not played This game.");
                }

                return bubbleGames;
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
