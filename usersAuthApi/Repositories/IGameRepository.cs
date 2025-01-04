using Microsoft.AspNetCore.Mvc;
using usersAuthApi.Models.DTO;

namespace usersAuthApi.Repositories
{
    public interface IGameRepository
    {
        Task<ActionResult<List<AllGamesResponseDto>>> GetAllGamesIndexAsync();
        Task<gameResponseDto> GetByIdAsync(int id);
        Task<gameResponseDto> UpdateGameAsync(int id, gameRequestDto requestDto);
        Task<gameResponseDto> AddGameAsync(gameRequestDto gameRequestDto);
        Task<bool> DeleteGameAsync(int id);
    }
}
