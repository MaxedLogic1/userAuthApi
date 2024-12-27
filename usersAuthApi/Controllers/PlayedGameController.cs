//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using usersAuthApi.Models.DTO;
//using usersAuthApi.Repositories;

//namespace usersAuthApi.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class PlayedGameController : ControllerBase
//    {

//        private readonly IPlayedGameRepository _PlayGameRepository;
//        public PlayedGameController(IPlayedGameRepository PlayGameRepository)
//        {
//            _PlayGameRepository = PlayGameRepository;
//        }

//        [HttpPost]
//        [Route("PlayedGame")]
//        public async Task<IActionResult> AddPlayGame([FromForm] PlayedGameRequestDto playGameRequestDto)
//        {
//            if (!ModelState.IsValid)
//            {
//                throw new ArgumentNullException("Model can't be null in Contorller");
//            }

//            if (playGameRequestDto == null)
//            {
//                throw new ArgumentNullException(nameof(playGameRequestDto), "Model can't be null in Contorller");

//            }
//            try
//            {
//                var playGame = await _PlayGameRepository.AddPlayGameAsync(playGameRequestDto);
//                return Ok(playGame);
//            }
//            catch (Exception ex)
//            {
//                return BadRequest(ex.Message);
//            }
//        }
//    }
//}
