using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using usersAuthApi.ApplicationDbContext;
using usersAuthApi.Exceptions;
using usersAuthApi.Models.Domain;
using usersAuthApi.Models.DTO;

namespace usersAuthApi.Repositories
{
    public class GameRepository : IGameRepository
    {
        private readonly userDbContext _userDbContext;
        public GameRepository(userDbContext userDbContext)
        {
            _userDbContext = userDbContext;
        }


        [HttpGet]
        [Route("GetAllGames")]
        public async Task<ActionResult<List<AllGamesResponseDto>>> GetAllGamesIndexAsync()
        {
            var gameDtos = _userDbContext.Tab_Games.Select(game => new AllGamesResponseDto
            {
                Id = game.Id,
                Name = game.Name,
                Type = game.Type,
                EntryDate = game.EntryDate,
                IsActive = game.IsActive
            }).ToList();

            return gameDtos;
        }
        public async Task<gameResponseDto> AddGameAsync(gameRequestDto gameRequestDto)
        {
            var checkGame = await _userDbContext.Tab_Games.AnyAsync(u => u.Name == gameRequestDto.Name);
            if (checkGame == null)
            {
                return null;
            }

            var newGame = new GamesModel
            {
                RandomId = $"game_{new Random().Next(1000, 9999)}",
                Name = gameRequestDto.Name,
                Description = gameRequestDto.Description,
                Type = gameRequestDto.Type,
                Target = gameRequestDto.Target,
                Profit = gameRequestDto.Profit,
                Percentage = gameRequestDto.Percentage,
                IsActive = true,
                EntryDate = DateTime.Now.AddMinutes(330)
            };

            _userDbContext.Tab_Games.Add(newGame);
            await _userDbContext.SaveChangesAsync();
            //return the Added Game
            var responseDto = new gameResponseDto
            {
                Id = newGame.Id,
                Name = newGame.Name,
                Type = newGame.Type,
                Description = newGame.Description,
                Target = newGame.Target,
                Profit = newGame.Profit,
                Percentage = newGame.Percentage,
                RandomId = newGame.RandomId,
                IsActive = newGame.IsActive,
                EntryDate = newGame.EntryDate,
            };

            return responseDto;
        }

        public async Task<gameResponseDto> GetByIdAsync(int id)
        {
            var game = await _userDbContext.Tab_Games.FirstOrDefaultAsync(g => g.Id == id);
            var responseDto = new gameResponseDto
            {
                Id = game.Id,
                Name = game.Name,
                Type = game.Type,
                Description = game.Description,
                Target = game.Target,
                Profit = game.Profit,
                Percentage = game.Percentage,
                RandomId = game.RandomId,
                IsActive = game.IsActive,
                EntryDate = game.EntryDate,
            };

            return responseDto;
        }

        public async Task<gameResponseDto> UpdateGameAsync(int id, [FromForm] gameRequestDto requestDto)
        {
            var game = await _userDbContext.Tab_Games.FindAsync(id);
            if (game == null)
            {
                return null;
            }
            // Update  game details
            game.Name = requestDto.Name;
            game.Description = requestDto.Description;
            game.Type = requestDto.Type;
            game.Target = requestDto.Target;
            game.Percentage = requestDto.Percentage;
            game.Profit = requestDto.Profit;
            game.EntryDate = DateTime.Now.AddMinutes(330);

            await _userDbContext.SaveChangesAsync();

            var updatedGame = new gameResponseDto
            {

                Id = game.Id,
                Name = game.Name,
                Type = game.Type,
                Description = game.Description,
                Target = game.Target,
                Percentage = game.Percentage,
                Profit = game.Profit,
                EntryDate = game.EntryDate,
                IsActive = game.IsActive,
                RandomId = game.RandomId,


            };
            return updatedGame;
        }


        public async Task<bool> DeleteGameAsync(int id)
        {
            var game = await _userDbContext.Tab_Games.FirstOrDefaultAsync(g => g.Id == id);

            if (game == null)
            {
                return false;
            }

            _userDbContext.Tab_Games.Remove(game);
            await _userDbContext.SaveChangesAsync();
            return true;


            //var responseDto = new gameResponseDto
            //{
            //    Id = game.Id,
            //    Name = game.Name,
            //    Type = game.Type,
            //    Description = game.Description,
            //    RandomId = game.RandomId,
            //    IsActive = game.IsActive,
            //    EntryDate = game.EntryDate,
            //};

            //return responseDto;
        }


    }
}

