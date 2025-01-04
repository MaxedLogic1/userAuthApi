using usersAuthApi.ApplicationDbContext;
using usersAuthApi.Models.DTO;

namespace usersAuthApi.Repositories
{
    public interface IFlappyBirdGameRepository
    {

      Task<FlappyBirdGameResponseDto> FlappyBirdGameResponse(FlappyBirdGameRequestDto flappyBirdGameRequestDto);
        Task<FlappyBirdGameResponseDto> FlappyBirdGameResponseTime(FlappyBirdGameTimeRequestDto flappyBirdTimeGameRequestDto);
     


    }
}
