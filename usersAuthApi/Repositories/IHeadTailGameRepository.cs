using usersAuthApi.ApplicationDbContext;
using usersAuthApi.Models.DTO;

namespace usersAuthApi.Repositories
{
    public interface IHeadTailGameRepository
    {

      Task<HeadTailGameResponseDto> HeadTailGameResponse(HeadTailGameReuestDto headTailGameReuestDto);


    }
}
