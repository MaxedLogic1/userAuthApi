using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using usersAuthApi.Models.DTO;
using usersAuthApi.Repositories;

namespace usersAuthApi.Controllers
{
    [Route("Api/[controller]")]
    [ApiController]
    public class BubbleGameController : ControllerBase
    {

        private readonly IBubbleGameRepository _bubbleGameRepository;

        public BubbleGameController(IBubbleGameRepository bubbleGameRepository)
        {
            _bubbleGameRepository = bubbleGameRepository;
        }

        [HttpPost("BubbleGame")]
        public async Task<IActionResult> PlayGame([FromForm] BubbleGameRequestDto bubbleGameRequestDto)
        {
            if (bubbleGameRequestDto == null)
            {
                return BadRequest("Invalid game data.");
            }
            try
            {
                var result = await _bubbleGameRepository.BubbleGameResponse(bubbleGameRequestDto);
                return Ok(new { Message = result });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = $"Custom Error: {ex.Message}" });
            }

        }
    }

}
