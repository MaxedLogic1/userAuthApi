using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using usersAuthApi.Models.DTO;
using usersAuthApi.Repositories;

namespace usersAuthApi.Controllers
{
    [Route("Api/[controller]")]
    [ApiController]
    public class GamesController : ControllerBase
    {
        private readonly IGameRepository _gameRepository;
        public GamesController(IGameRepository gameRepository)
        {
            _gameRepository = gameRepository;
        }

        // Add New Game in Database
        [HttpPost]
        [Route("AddGame")]
        public async Task<IActionResult> AddGame([FromForm] gameRequestDto gameRequestDto)
        {
            if (gameRequestDto == null)
            {
                throw new ArgumentNullException(nameof(gameRequestDto), "Model can't be null in Contorller");
            }

            try
            {
                var game = await _gameRepository.AddGameAsync(gameRequestDto);
                return Ok(game);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        // Get Game by Id in the Database
        [HttpPost]
        [Route("GetById/{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            if(id <= 0)
            {
                return BadRequest("Invalid Game ID.");
            }
            try
            {
                var GetGame = await _gameRepository.GetByIdAsync(id);
                return Ok(GetGame);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while Updating the game.", error = ex.Message });
            }
        }



        // Update/Put Game by Id in the Database
        [HttpPut]
        [Route("UpdateGame/{id}")]
        public async Task<IActionResult> UpdateGame( int id, [FromForm] gameRequestDto requestDto)
        {
            if (id == null)
            {
                return BadRequest("Invalid game data.");
            }

            var gameExists = await _gameRepository.GetByIdAsync(id);
            if (gameExists == null)
            {
                return NotFound(new { message = "Game not found." });
            }

            var result = await _gameRepository.UpdateGameAsync(id, requestDto);

            var updatedGame = await _gameRepository.GetByIdAsync(id);
            return Ok(updatedGame); // Return the updated game
        }



        // Delete Game by Id in the Database
        [HttpDelete]
        [Route("DeleteGame/{id}")]
        public async Task<IActionResult> DeleteGame(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid game ID.");
            }

            try
            {
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
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while deleting the game.", error = ex.Message });
            }

        }
    }
}
