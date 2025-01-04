using Microsoft.AspNetCore.Mvc;
using usersAuthApi.Exceptions;
using usersAuthApi.Models.DTO;
using usersAuthApi.Repositories;

namespace usersAuthApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class BubbleGameController : ControllerBase
    {
        private readonly IPlayerTotalAmountRepository _playerTotalAmountRepository;
        private readonly IPlayerAndGameRepository _playerAndGameRepository;
        private readonly IBubbleGameRepository _bubbleGameRepository;

        public BubbleGameController(IBubbleGameRepository bubbleGameRepository,
                IPlayerTotalAmountRepository playerTotalAmountRepository,
            IPlayerAndGameRepository playerAndGameRepository)
        {
            _bubbleGameRepository = bubbleGameRepository;
            _playerAndGameRepository = playerAndGameRepository;
            _playerTotalAmountRepository = playerTotalAmountRepository;
        }

        [HttpPost]
        [Route("PlayeBubbleGame")]
        //[ValidateModel]
        public async Task<IActionResult> PlayeBubbleGame([FromForm] BubbleGameRequestDto bubbleGameRequestDto)
        {
            try
            {
                if (bubbleGameRequestDto.GId <= 0 || bubbleGameRequestDto.PId <= 0 || bubbleGameRequestDto == null)
                {
                    return BadRequest("Model can't be null: ");
                }

              //Check Player and game exits 
                var playeAndGameExits = await _playerAndGameRepository.GetPlayerAndGameAsync(bubbleGameRequestDto.PId, bubbleGameRequestDto.GId);
                if (!playeAndGameExits)
                {
                    throw new PlayerNotFoundCustomException();
                }

                //Get Player Name
                var playerName = await _playerAndGameRepository.GetPlayerName(bubbleGameRequestDto.PId);

                //Total Amount is less then bet amount 
                var totalAmount = await _playerTotalAmountRepository.PlayerTotalAmount(bubbleGameRequestDto.PId);

                if (totalAmount >= bubbleGameRequestDto.BetAmount)
                {
                    //Call Game Method 
                    var result = await _bubbleGameRepository.BubbleGameResponse(bubbleGameRequestDto);
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
                    return Ok(new { Message = $"{playerName} total amount is Greater then Bet Amount", });
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
