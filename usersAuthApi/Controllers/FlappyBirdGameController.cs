using Microsoft.AspNetCore.Mvc;
using usersAuthApi.Exceptions;
using usersAuthApi.Models.DTO;
using usersAuthApi.Repositories;

namespace usersAuthApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class FlappyBirdGameController : ControllerBase
    {
        private readonly IPlayerTotalAmountRepository _playerTotalAmountRepository;
        private readonly IPlayerAndGameRepository _playerAndGameRepository;
        private readonly IFlappyBirdGameRepository _flappyBirdGameRepository;

        public FlappyBirdGameController(IFlappyBirdGameRepository flappyBirdGameRepository,
            IPlayerTotalAmountRepository playerTotalAmountRepository,
            IPlayerAndGameRepository playerAndGameRepository)
        {
            _flappyBirdGameRepository = flappyBirdGameRepository;
            _playerTotalAmountRepository = playerTotalAmountRepository;
            _playerAndGameRepository = playerAndGameRepository;
        }

        [HttpPost]
        [Route("PlayFlappyBirdGame")]
        //[ValidateModel]
        public async Task<IActionResult> PlayeFlappyBirdGame([FromForm] FlappyBirdGameRequestDto flappyBirdGameRequestDto)
        {
            try
            {
                if (flappyBirdGameRequestDto.GId <= 0 || flappyBirdGameRequestDto.PId <= 0 || flappyBirdGameRequestDto == null)
                {
                    return BadRequest("Model can't be null: ");
                }
                var playeAndGameExits = await _playerAndGameRepository.GetPlayerAndGameAsync(flappyBirdGameRequestDto.PId, flappyBirdGameRequestDto.GId);
                if (!playeAndGameExits)
                {
                    throw new PlayerNotFoundCustomException();
                }

                var playerName = await _playerAndGameRepository.GetPlayerName(flappyBirdGameRequestDto.PId);

                if (playerName == null)
                {
                    throw new PlayerNotFoundCustomException();
                }

                var totalAmount = await _playerTotalAmountRepository.PlayerTotalAmount(flappyBirdGameRequestDto.PId);

                if (totalAmount >= flappyBirdGameRequestDto.BetAmount)
                {
                    var result = await _flappyBirdGameRepository.FlappyBirdGameResponse(flappyBirdGameRequestDto);
                    if (result.Message == "Win")
                    {
                        return Ok(new { Message = $"Congratulation {playerName} You are win!", });
                    }
                    else if (result.Message == "Loss")
                    {
                        return Ok(new { Message = $"Sorry {playerName} you loss! " });
                    }
                    else
                    {
                        return Ok(new { Message = $"Sorry somthing went wrong!" });
                    }
                }
                else
                {
                    return Ok(new { Message = $"{playerName} total amount is greater then Bet Amount", });
                }
            }
            catch (GameNotFoundCustomException)
            {
                throw new GameNotFoundCustomException("Game Does't exsit: ");
            }
            catch (PlayerNotFoundCustomException)
            {
                throw new GameNotFoundCustomException("Player Does't exsit: ");
            }

        }


        [HttpPost]
        [Route("PlayFlappyBirdGameUsingTime")]
        //[ValidateModel]
        public async Task<IActionResult> PlayeFlappyBirdGameTime([FromForm] FlappyBirdGameTimeRequestDto flappyBirdGameTimeRequestDto)
        {
            try
            {

                flappyBirdGameTimeRequestDto.StartGameTime = flappyBirdGameTimeRequestDto.StartGameTime.AddMinutes(330);
                flappyBirdGameTimeRequestDto.EndGameTime = flappyBirdGameTimeRequestDto.EndGameTime.AddMinutes(330);
                if (flappyBirdGameTimeRequestDto.GId <= 0 || flappyBirdGameTimeRequestDto.PId <= 0 || flappyBirdGameTimeRequestDto == null)
                {
                    return BadRequest("Model can't be null: ");
                }
                var playeAndGameExits = await _playerAndGameRepository.GetPlayerAndGameAsync(flappyBirdGameTimeRequestDto.PId, flappyBirdGameTimeRequestDto.GId);
                if (!playeAndGameExits)
                {
                    throw new PlayerNotFoundCustomException();
                }

                var playerName = await _playerAndGameRepository.GetPlayerName(flappyBirdGameTimeRequestDto.PId);

                if (playerName == null)
                {
                    throw new PlayerNotFoundCustomException();
                }

                var totalAmount = await _playerTotalAmountRepository.PlayerTotalAmount(flappyBirdGameTimeRequestDto.PId);

                if (totalAmount >= flappyBirdGameTimeRequestDto.BetAmount)
                {
                    var result = await _flappyBirdGameRepository.FlappyBirdGameResponseTime(flappyBirdGameTimeRequestDto);
                    if (result.Message == "Win")
                    {
                        return Ok(new { Message = $"Congratulation {playerName} You are win!", });
                    }
                    else if (result.Message == "Loss")
                    {
                        return Ok(new { Message = $"Sorry {playerName} you loss! " });
                    }
                    else
                    {
                        return Ok(new { Message = $"Sorry somthing went wrong!" });
                    }
                }
                else
                {
                    return Ok(new { Message = $"{playerName} total amount is greater then Bet Amount", });
                }
            }
            catch (GameNotFoundCustomException)
            {
                throw new GameNotFoundCustomException("Game Does't exsit: ");
            }
            catch (PlayerNotFoundCustomException)
            {
                throw new GameNotFoundCustomException("Player Does't exsit: ");
            }

        }
    }

}
