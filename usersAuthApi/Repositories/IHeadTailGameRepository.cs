using usersAuthApi.Models.DTO;

namespace usersAuthApi.Repositories
{
    public interface IHeadTailGameRepository
    {

      Task<HeadTailGameResponseDto> HeadTailGameResponse(HeadTailGameReuestDto headTailGameReuestDto);


        Task<List<HeadTailGameResponseDto>> MultiUserHeadTailGameResponse(HeadTailGameMultiUsersRequestDto headTailGameMultiUsersRequestDto);
    }
}
