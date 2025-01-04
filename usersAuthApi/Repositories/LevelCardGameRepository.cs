using System.Numerics;
using Microsoft.EntityFrameworkCore;
using usersAuthApi.ApplicationDbContext;
using usersAuthApi.Exceptions;
using usersAuthApi.Models.Domain;
using usersAuthApi.Models.DTO;

namespace usersAuthApi.Repositories
{
    public class LevelCardGameRepository : ILevelCardGameRepository
    {
        private readonly userDbContext _userDbContext;

        public LevelCardGameRepository(userDbContext userDbContext)
        {
            _userDbContext = userDbContext;
        }

        public async Task<LevelCardGameResponseDto> BubbleGameResponse(LevelCardGameRequestDto levelCardGameRequestDto)
        {

            //player is exsit
            var player = await _userDbContext.Tab_Register
              .Where(u => u.Id == levelCardGameRequestDto.PId)
              .FirstOrDefaultAsync();
            if (player == null)
            {
                throw new GameNotFoundCustomException("Player Does't exsit: ");
            }

            //find the game 
            var game = await _userDbContext.Tab_Games
                .Where(g => g.Id == levelCardGameRequestDto.GId)
                .FirstOrDefaultAsync();
            if (game == null)
            {
                throw new GameNotFoundCustomException("Game Does't exsit: ");
            }

            //Player totalCreditAmount 
            decimal creditAmount = await _userDbContext.Tab_FundTransaction
                .Where(f => f.PId == levelCardGameRequestDto.PId)
                .SumAsync(f => (decimal?)f.CreditAmount) ?? 0;

            //Player totalCreditAmount 
            decimal debitAmount = await _userDbContext.Tab_FundTransaction
               .Where(f => f.PId == levelCardGameRequestDto.PId)
               .SumAsync(f => (decimal?)f.DebitAmount) ?? 0;

            //Player MinimumAmount  
            decimal minAmount = creditAmount - debitAmount;

            if (minAmount < 10)
            {
                return new LevelCardGameResponseDto
                {
                    PId = levelCardGameRequestDto.PId,
                    GId = levelCardGameRequestDto.GId,
                    BetAmount = levelCardGameRequestDto.BetAmount,
                    Message = "Minimum balance required.Atleast $ 10 required ",
                    TotalAmount = minAmount,
                    EntryDate = DateTime.Now.AddMinutes(330),
                };
            }

            //Target Come from DataBase
            var target = await _userDbContext.Tab_Games
                  .Where(g => g.Id == levelCardGameRequestDto.GId)
                  .Select(g => g.Target)   
                  .FirstOrDefaultAsync();

            //Profit Come From GameTable
            //var profit = await _userDbContext.Tab_Games
            //     .Where(g => g.Id == levelCardGameRequestDto.GId)
            //     .Select(g => g.Profit)
            //     .FirstOrDefaultAsync();

            //Profit Percentage 
            var profitPercent = await _userDbContext.Tab_Games
               .Where(g => g.Id == levelCardGameRequestDto.GId)
               .Select(g => g.Percentage)
               .FirstOrDefaultAsync();

            //assign betAmount
            decimal betAmount = levelCardGameRequestDto.BetAmount;

            //Achive target is greter than target
            bool isWin = levelCardGameRequestDto.AchiveTarget >= target;

            //Decide result base on Achivet target
            decimal resultAmount = isWin ? betAmount * profitPercent : betAmount;

            //Set messaget to save in the remark section 
            string resultMessage = isWin ? $"Congratulations, You Win: {resultAmount}" : $"Sorry, You Lost: {resultAmount}";

            //Total Amount 
            decimal currentAmount = creditAmount - debitAmount;

            //After win/loss Current amount
            currentAmount = isWin ? currentAmount + resultAmount : currentAmount - resultAmount;

            //Add New Row in the tab_fund table 
            var fundTransaction = new FundTransactionModel
            {
                GId = levelCardGameRequestDto.GId,
                PId = levelCardGameRequestDto.PId,
                CreditAmount = isWin ? resultAmount : 0,
                DebitAmount = isWin ? 0 : resultAmount,
                Remark = resultMessage,
                Type = isWin ? "win" : "loss",
                TransactionDate = DateTime.Now.AddMinutes(330),
                TxNoId = $"TX_{new Random().Next(1000, 9999)}",
                Images = null
            };

            //save new row in tab_fund table 
            await _userDbContext.Tab_FundTransaction.AddAsync(fundTransaction);
            await _userDbContext.SaveChangesAsync();

            //save new row to Game index table 
            var newPlayGame = new LevelCardGameIndex
            {
                PId = levelCardGameRequestDto.PId,
                GId = levelCardGameRequestDto.GId,
                BetAmount = levelCardGameRequestDto.BetAmount,
                Type = isWin ? "Win" : "Loss",
                Win = isWin ? resultAmount : 0,
                Loss = isWin ? 0 : resultAmount,
                Remark = $"Game: {game.Name}, Player: {player.UserName} ",
                IsActive = true,
                RandomId = $"LCGame_{new Random().Next(1000, 9999)}",
                EntryDate = DateTime.Now.AddMinutes(330)
            };

            //save new row in Tab_BubbleGameIndex table 
         //   await _userDbContext.Tab_LevelCardGameIndex.AddAsync(newPlayGame);
            await _userDbContext.SaveChangesAsync();

            //return to the user and you want to remove this
            var responseDto = new LevelCardGameResponseDto
            {
                PId = levelCardGameRequestDto.PId,
                GId = levelCardGameRequestDto.GId,
                Message = resultMessage,
                BetAmount = betAmount,
                EntryDate = DateTime.Now.AddMinutes(330),
                TotalAmount = currentAmount

            };
            // return user 
            return responseDto;
        }
         
        public async Task<string> GetPlayerName(LevelCardGameRequestDto levelCardGameRequestDto)
        {
            //player is exsit
            var player = await _userDbContext.Tab_Register
              .Where(u => u.Id == levelCardGameRequestDto.PId)
              .FirstOrDefaultAsync();
            if (player == null)
            {
                return null;
            }
            else
            {
                return player.Name;
            }
           
        }
    }
}
