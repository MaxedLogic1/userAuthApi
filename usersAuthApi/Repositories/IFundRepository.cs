using usersAuthApi.Models.DTO;

namespace usersAuthApi.Repositories
{
    public interface IFundRepository
    {
        Task<fundTransectionResponseDto> AddFundTransactionAsync(fundTransactionRequestDto fundTransactionDto);
    }
}
