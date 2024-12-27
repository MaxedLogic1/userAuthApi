using Microsoft.AspNetCore.Mvc;
using usersAuthApi.Models.DTO;
using usersAuthApi.Repositories;
using Microsoft.AspNetCore.Http;
using static System.Net.Mime.MediaTypeNames;
using usersAuthApi.ApplicationDbContext;

namespace usersAuthApi.Controllers
{
    [Route("Api/FundTransaction")]
    [ApiController]
    public class fundController : ControllerBase
    {
        private readonly IFundRepository _fundRepository;
        public fundController(IFundRepository fundRepository)
        {
            _fundRepository = fundRepository;
        }


        [HttpPost]
        [Route("AddFund")]
        public async Task<ActionResult> AddFundTransaction([FromForm] fundTransactionRequestDto requestDto)
        {
            if (requestDto == null)
            {
                throw new InvalidOperationException("Add Fund Can't Be Empty");
            }

            try
            {
                var responseDto = await _fundRepository.AddFundTransactionAsync(requestDto);
                return Ok(responseDto);
            }
            catch (Exception)
            {
                throw new InvalidOperationException("Add Fund Can't Be Empty");
            }
        }
    }
}
