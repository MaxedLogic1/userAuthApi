using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using usersAuthApi.Models.DTO;
using usersAuthApi.Repositories;

namespace usersAuthApi.Controllers
{
    [Route("Api/[controller]")]
    [ApiController]
    public class HeadTailGameController : ControllerBase
    {

        private readonly IHeadTailGameRepository _headTailGameRepository;

        public HeadTailGameController(IHeadTailGameRepository headTailGameRepository)
        {
            _headTailGameRepository = headTailGameRepository;
        }

        [HttpPost("PlaySingleUsers")]
        public async Task<IActionResult> PlayHeadTailGame([FromForm] HeadTailGameReuestDto headTailGameReuestDto)
        {
            var curentDateTime = DateTime.UtcNow;

            headTailGameReuestDto.BetEntryDate = curentDateTime;
            if (headTailGameReuestDto == null)
            {
                return BadRequest("Invalid game data.");
            }
            try
            {
                var result = await _headTailGameRepository.HeadTailGameResponse(headTailGameReuestDto);

                return Ok(new { Message = result });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = $"Custom Error: {ex.Message}" });
            }

        }

        [HttpPost]
        [Route("PlayMultipleUsers")]
        public async Task<IActionResult> PlayMultipleUsers([FromBody] HeadTailGameMultiUsersRequestDto headTailGameMultiUsersRequestDtos)
        {

            if (headTailGameMultiUsersRequestDtos == null || headTailGameMultiUsersRequestDtos.Bets.Count == 0)
            {
                return BadRequest("No bet data provided.");
            }
            var betTimeToNow = DateTime.Now;

            foreach (var betRequest in headTailGameMultiUsersRequestDtos.Bets)
            {
                betRequest.BetEntryDate = betTimeToNow;

            }
            try
            {
                var responses = await _headTailGameRepository.MultiUserHeadTailGameResponse(headTailGameMultiUsersRequestDtos);

                return Ok(responses);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while processing the request.", details = ex.Message });
            }
        }
    }
}

