using Microsoft.AspNetCore.Mvc;
using usersAuthApi.Models.DTO;
using usersAuthApi.Repositories;
using Microsoft.AspNetCore.Http;
using static System.Net.Mime.MediaTypeNames;
using usersAuthApi.ApplicationDbContext;
using usersAuthApi.Exceptions;

namespace usersAuthApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class FundTrasectionController : ControllerBase
    {
        private readonly IFundRepository _fundRepository;
        public FundTrasectionController(IFundRepository fundRepository)
        {
            _fundRepository = fundRepository;
        }


        [HttpPost]
        [Route("AddFund")]
        public async Task<ActionResult> AddFundTransaction([FromForm] fundTransactionRequestDto requestDto)
        {
            try
            {
                if (requestDto.PId == null || requestDto.PId <=0)
                {
                    throw new PlayerNotFoundCustomException ("Plyer Not Found");
                }
                var responseDto = await _fundRepository.AddFundTransactionAsync(requestDto);
                if (responseDto != null)
                {
                    return Ok(new { Message = $"Fund added to the {requestDto.PId} successfully: " });
                }
                else
                {
                    return Ok(new { Message = $"Fund not added to the {requestDto.PId}: " });
                }
            }
            catch (PlayerNotFoundCustomException)
            {
                throw new PlayerNotFoundCustomException("Uesr Not Found");
            }
        }
    }
}
