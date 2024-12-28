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

        [HttpPost("HeadTailGame")]
        public async Task<IActionResult> PlayHeadTailGame([FromForm] HeadTailGameReuestDto headTailGameReuestDto)
        {
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
    }

}
