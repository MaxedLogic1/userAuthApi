using usersAuthApi.ApplicationDbContext;
using usersAuthApi.Models.DTO;

namespace usersAuthApi.Repositories
{
    public interface ILevelCardGameRepository
    {

      Task<LevelCardGameResponseDto> BubbleGameResponse(LevelCardGameRequestDto levelCardGameRequestDto);
      Task<string> GetPlayerName(LevelCardGameRequestDto levelCardGameRequestDto);


    }
}
