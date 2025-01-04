using usersAuthApi.Models.DTO;

namespace usersAuthApi.Repositories
{
    public interface IPlayerTotalAmountRepository
    {
        Task<decimal> PlayerTotalAmount(int PId );
    }
}
