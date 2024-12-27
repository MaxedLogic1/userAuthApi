using System.Numerics;
using Microsoft.EntityFrameworkCore;
using usersAuthApi.ApplicationDbContext;
using usersAuthApi.Models.Domain;
using usersAuthApi.Models.DTO;

namespace usersAuthApi.Repositories
{
    public class BubbleGameRepository : IBubbleGameRepository
    {
        private readonly userDbContext _userDbContext;

        public BubbleGameRepository(userDbContext userDbContext)
        {
            _userDbContext = userDbContext;
        }
    
        public async Task<BubbleGameResponseDto> BubbleGameResponse(BubbleGameRequestDto bubbleGameRequestDto)
        {


            if (bubbleGameRequestDto == null)
            {
                throw new ArgumentNullException(nameof(bubbleGameRequestDto), "Request data cannot be null.");
            }
            //player is exsit
            var player = await _userDbContext.Tab_Register
              .Where(u => u.Id == bubbleGameRequestDto.PId)
              .FirstOrDefaultAsync();

            if (player == null)
            {
                throw new InvalidOperationException("Player not found.");
            }
            //find the game 
            var game = await _userDbContext.Tab_Games
                .Where(g => g.Id == bubbleGameRequestDto.GId)
                .FirstOrDefaultAsync();
            //check the game is null or not 
            if (game == null)
            {
                throw new InvalidOperationException("Game not found.");
            }


            decimal creditAmount = await _userDbContext.Tab_FundTransaction
                .Where(f => f.PId == bubbleGameRequestDto.PId)
                .SumAsync(f => (decimal?)f.CreditAmount) ?? 0;

            decimal debitAmount = await _userDbContext.Tab_FundTransaction
               .Where(f => f.PId == bubbleGameRequestDto.PId)
               .SumAsync(f => (decimal?)f.DebitAmount) ?? 0;

            decimal minAmount = creditAmount - debitAmount;
            if (minAmount < 10)
            {
                return new BubbleGameResponseDto
                {
                    PId = bubbleGameRequestDto.PId,
                    GId = bubbleGameRequestDto.GId,
                    BetAmount = bubbleGameRequestDto.BetAmount,
                    Message = "Minimum balance required.Atleast $ 10 required ",
                    TotalAmount = minAmount,
                    EntryDate = DateTime.Now,
                };
            }

            int target = 10;

            bool isWin = bubbleGameRequestDto.AchiveTarget >= target;

            decimal betAmount = bubbleGameRequestDto.BetAmount;

            decimal resultAmount = isWin ? betAmount * 2 : betAmount;



            string resultMessage = isWin ? $"Congratulations, You Win: {resultAmount}" : $"Sorry, You Lost: {resultAmount}";

            //Total Amount 
            decimal currentAmount = creditAmount - debitAmount;
            //After win/loss Current amount
            currentAmount = isWin ? currentAmount + resultAmount : currentAmount - resultAmount;

            //update the table 
            var fundTransaction = new FundTransactionModel
            {
                PId = bubbleGameRequestDto.PId,
                CreditAmount = isWin ? resultAmount : 0,
                DebitAmount = isWin ? 0 : resultAmount,
                Remark = resultMessage,
                Type = isWin ? "win" : "loss",
                TransactionDate = DateTime.Now,
                TxNoId = $"TX_{new Random().Next(1000, 9999)}",
                Images = null
            };

            await _userDbContext.Tab_FundTransaction.AddAsync(fundTransaction);
            await _userDbContext.SaveChangesAsync();

            //var playerName = await _userDbContext.Tab_Register
            //            .Where(u => u.Id == bubbleGameRequestDto.PId)
            //            .Select(u => u.UserName)  
            //            .FirstOrDefaultAsync();
            //response Dto to sho on the response body

            var newPlayGame = new BubbleGameIndexModel
            {
                RandomId = $"PGame_{new Random().Next(1000, 9999)}",
                PId = bubbleGameRequestDto.PId,
                GId = bubbleGameRequestDto.GId,
                BetAmount = bubbleGameRequestDto.BetAmount,
                WinLoss = isWin ? "win":"loss",
                Remark = $"Game: {game.Name}, Player: {player.UserName} ",
                IsActive = true,
                EntryDate = DateTime.Now
            };

            await _userDbContext.Tab_BubbleGameIndex.AddAsync(newPlayGame);
            await _userDbContext.SaveChangesAsync();




            var responseDto = new BubbleGameResponseDto
            {
                PId = bubbleGameRequestDto.PId,
                GId = bubbleGameRequestDto.GId,
                Message = resultMessage,
                BetAmount = betAmount,
                EntryDate = DateTime.Now,
                TotalAmount = currentAmount

            };
            //await _userDbContext.Tab_BubbleGameIndex.AddAsync(responseDto);
            //await _userDbContext.SaveChangesAsync();
            return responseDto;
        }
    }
}
