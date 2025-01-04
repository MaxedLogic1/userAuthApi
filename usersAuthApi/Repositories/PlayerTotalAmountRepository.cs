using Microsoft.EntityFrameworkCore;
using usersAuthApi.ApplicationDbContext;
using usersAuthApi.Models.DTO;

namespace usersAuthApi.Repositories
{
    public class PlayerTotalAmountRepository : IPlayerTotalAmountRepository
    {
        private readonly userDbContext _userDbContext;
        public PlayerTotalAmountRepository(userDbContext userDbContext)
        {
            _userDbContext = userDbContext;
        }
        public async Task<decimal> PlayerTotalAmount(int PId)
        {
            var player = await _userDbContext.Tab_FundTransaction
               .Where(f => f.PId == PId)
               .FirstOrDefaultAsync();
            if (player == null)
            {
                throw new InvalidOperationException("Player not found.");
            }
            //Player totalCreditAmount 
            decimal creditAmount = await _userDbContext.Tab_FundTransaction
                .Where(f => f.PId == PId)
                .SumAsync(f => (decimal?)f.CreditAmount) ?? 0;

            //Player totalCreditAmount 
            decimal debitAmount = await _userDbContext.Tab_FundTransaction
               .Where(f => f.PId == PId)
               .SumAsync(f => (decimal?)f.DebitAmount) ?? 0;

            //Player MinimumAmount  
            decimal totalAmount = creditAmount - debitAmount;

            return totalAmount;
        }
    }
}

