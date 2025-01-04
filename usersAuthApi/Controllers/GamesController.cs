using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using usersAuthApi.ApplicationDbContext;
using usersAuthApi.Exceptions;
using usersAuthApi.Models.DTO;
using usersAuthApi.Repositories;

namespace usersAuthApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class GamesController : ControllerBase
    {
        private readonly IGameRepository _gameRepository;
        public GamesController(IGameRepository gameRepository)
        {
            _gameRepository = gameRepository;
        }
        [HttpGet]
        [Route("AllGames")]
        public async Task<ActionResult<List<AllGamesResponseDto>>> GetAllGamesIndex()
        {
            try
            {
                var allGames = await _gameRepository.GetAllGamesIndexAsync();

                if (allGames == null)
                {
                    return NotFound(new { Message = "No games found." });
                }

                return Ok(allGames);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An unexpected error occurred while processing the game data.", Error = ex.Message });
            }
        }

        [HttpPost]
        [Route("AddNewGame")]
        public async Task<IActionResult> AddGame([FromForm] gameRequestDto gameRequestDto)
        {
            try
            {
                if (gameRequestDto == null)
                {
                    throw new GameNotFoundCustomException("Model does not null: ");
                }
                var game = await _gameRepository.AddGameAsync(gameRequestDto);
                if(game != null)
                {
                    return Ok(new { Message = $"{gameRequestDto.Name} added successfull: " });
                }
                else
                {
                    return Ok(new { Message = $"{gameRequestDto.Name} already exist or not added : " });
                }
               
            }
            catch (GameNotFoundCustomException)
            {
                throw new GameNotFoundCustomException("Game does not exist : ");
            }
        }


        
        [HttpGet]
        [Route("GetById/{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
           
            try
            {
                if (id == null||id <= 0)
                {
                    throw new GameNotFoundCustomException("Game does not exist: ");
                }
                var GetGame = await _gameRepository.GetByIdAsync(id);
                if (GetGame != null)
                {
                    return Ok(GetGame);
                }
                else
                {
                    return Ok(new { Message = $"{id} no game exists: " });
                }
                
            }
            catch (GameNotFoundCustomException)
            {
                throw new GameNotFoundCustomException("Game Not Found.");
            }
        }



        
        [HttpPut]
        [Route("UpdateGame/{id}")]
        public async Task<IActionResult> UpdateGame(int id, [FromForm] gameRequestDto requestDto)
        {
           
            try
            {
                if (id == null ||id <= 0)
                {
                    throw new GameNotFoundCustomException("Game id is invalid: ");
                }
                var gameExists = await _gameRepository.GetByIdAsync(id);
                 
                if (gameExists != null)
                {
                    var result = await _gameRepository.UpdateGameAsync(id, requestDto);
                    var updatedGame = await _gameRepository.GetByIdAsync(id);
                    return Ok(updatedGame); // Return the updated game
                }
                else
                {
                    return Ok(new { Message = $"{id} no game exists: " });
                }
            }
            catch (GameNotFoundCustomException)
            {
                throw new GameNotFoundCustomException("No Game found ");
            }


        }

        
        [HttpDelete]
        [Route("DeleteGame/{id}")]
        public async Task<IActionResult> DeleteGame(int id)
        {
           

            try
            {
                if (id == null || id <= 0)
                {
                    throw new GameNotFoundCustomException("Id is invalid: ");
                }
                var result = await _gameRepository.DeleteGameAsync(id);

                if (result)
                {
                    return Ok(new { message = "Game deleted successfully." });
                }
                else
                {
                    return NotFound(new { message = "Game not found." });
                }
            }
            catch (GameNotFoundCustomException)
            {
                throw new GameNotFoundCustomException("Game Not Found.");
            }

        }
    }
}
