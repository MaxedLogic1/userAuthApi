using usersAuthApi.Models.DTO;

namespace usersAuthApi.Repositories
{
    public interface IGameRepository
    {
        Task<gameResponseDto> GetByIdAsync(int id);
        Task<gameResponseDto> UpdateGameAsync(int id, gameRequestDto requestDto);
        Task<gameResponseDto> AddGameAsync(gameRequestDto gameRequestDto);
        Task<bool> DeleteGameAsync(int id);
    }
}
