using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using usersAuthApi.Exceptions;
using usersAuthApi.Models.DTO;
using usersAuthApi.Repositories;

namespace usersAuthApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class HeadTailGameController : ControllerBase
    {
        private readonly IPlayerTotalAmountRepository _playerTotalAmountRepository;
        private readonly IPlayerAndGameRepository _playerAndGameRepository;
        private readonly IHeadTailGameRepository _headTailGameRepository;

        public HeadTailGameController(IHeadTailGameRepository headTailGameRepository,
              IPlayerTotalAmountRepository playerTotalAmountRepository,
            IPlayerAndGameRepository playerAndGameRepository)
        {
            _headTailGameRepository = headTailGameRepository;
            _playerAndGameRepository = playerAndGameRepository;
            _playerTotalAmountRepository = playerTotalAmountRepository;
        }

        [HttpPost("PlaySingleUsers")]
        public async Task<IActionResult> PlaySingleUsers([FromForm] HeadTailGameReuestDto headTailGameReuestDto)
        {
           
            try
            {
                //var curentDateTime = DateTime.UtcNow.AddMinutes(330);
                //headTailGameReuestDto.BetEntryDate = curentDateTime;

                if (headTailGameReuestDto == null|| headTailGameReuestDto.PId<=0 || headTailGameReuestDto.GId <=0)
                {
                    throw new ArgumentException("Model does not nulll: ");
                }

                //Check Player and game exits 
                var playeAndGameExits = await _playerAndGameRepository.GetPlayerAndGameAsync(headTailGameReuestDto.PId, headTailGameReuestDto.GId);
                if (!playeAndGameExits)
                {
                    throw new PlayerNotFoundCustomException();
                }

                //Get Player Name
                var playerName = await _playerAndGameRepository.GetPlayerName(headTailGameReuestDto.PId);

                var totalAmount = await _playerTotalAmountRepository.PlayerTotalAmount(headTailGameReuestDto.PId);

                if (totalAmount >= headTailGameReuestDto.BetAmount)
                {
                    //Call Game Method 
                    var result = await _headTailGameRepository.HeadTailGameResponse(headTailGameReuestDto);
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
                    return Ok(new { Message = $"{playerName} total amount is less then Bet Amount", });
                }
            }
            catch (GameNotFoundCustomException)
            {
                throw new GameNotFoundCustomException("Game id invalid: ");
            }
            catch (PlayerNotFoundCustomException)
            {
                throw new PlayerNotFoundCustomException("Player id invalid: ");
            }

        }

        [HttpPost]
        [Route("PlayMultipleUsers")]
        public async Task<IActionResult> PlayMultipleUsers([FromBody] HeadTailGameMultiUsersRequestDto headTailGameMultiUsersRequestDtos)
        {
            try
            {
                if (headTailGameMultiUsersRequestDtos == null || headTailGameMultiUsersRequestDtos.Bets.Count == 0)
                {
                    throw new GameNotFoundCustomException("Player Id Not Found.");
                }
                var betTimeToNow = DateTime.Now.AddMinutes(330);

                foreach (var betRequest in headTailGameMultiUsersRequestDtos.Bets)
                {
                    betRequest.BetEntryDate = betTimeToNow;
                }
                var responses = await _headTailGameRepository.MultiUserHeadTailGameResponse(headTailGameMultiUsersRequestDtos);
                if(responses != null)
                {
                    return Ok(new { Message = "Results: ", responses });
                }
                else
                {
                    return Ok(new { Message = " No result found: " });
                }
            }
            catch (GameNotFoundCustomException)
            {
                throw new GameNotFoundCustomException("Game Id Not Found.");
            }
            catch (PlayerNotFoundCustomException)
            {
                throw new PlayerNotFoundCustomException("Player Id Not Found.");
            }
        }
    }
}

