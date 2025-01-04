using usersAuthApi.ApplicationDbContext;
using usersAuthApi.Models.DTO;

namespace usersAuthApi.Repositories
{
    public interface IBubbleGameRepository
    {

      Task<BubbleGameResponseDto> BubbleGameResponse(BubbleGameRequestDto bubbleGameRequestDto);
      


    }
}
