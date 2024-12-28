using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;
using Microsoft.EntityFrameworkCore;
using usersAuthApi.ApplicationDbContext;
using usersAuthApi.Models.Domain;
using usersAuthApi.Models.DTO;

namespace usersAuthApi.Repositories
{
    public class HeadTailGameRepository : IHeadTailGameRepository
    {
        private readonly userDbContext _userDbContext;

        public HeadTailGameRepository(userDbContext userDbContext)
        {
            _userDbContext = userDbContext;
        }

        public async Task<HeadTailGameResponseDto> HeadTailGameResponse(HeadTailGameReuestDto headTailGameReuestDto)
        {
            if (headTailGameReuestDto == null)
            {
                throw new InvalidOperationException("HeadTail Model Can't be Null ?");
            }
            //player is exsit
            var player = await _userDbContext.Tab_Register
              .Where(u => u.Id == headTailGameReuestDto.PId)
              .FirstOrDefaultAsync();
            if (player == null)
            {
                throw new InvalidOperationException("Player not found.");
            }

            //find the game 
            var game = await _userDbContext.Tab_Games
                .Where(g => g.Id == headTailGameReuestDto.GId)
                .FirstOrDefaultAsync();
            //check the game is null or not 
            if (game == null)
            {
                throw new InvalidOperationException("Game not found.");
            }

            //Total Amount 
            decimal totalAmount = await _userDbContext.Tab_FundTransaction
                 .Where(f => f.PId == headTailGameReuestDto.PId)
                 .GroupBy(f => 1)
                 .Select(g => (decimal?)(g.Sum(f => (decimal?)f.CreditAmount) ?? 0) - (g.Sum(f => (decimal?)f.DebitAmount) ?? 0))
                 .FirstOrDefaultAsync() ?? 0;

            if (totalAmount <= headTailGameReuestDto.BetAmount)
            {
                return new HeadTailGameResponseDto
                {
                    BetAmount = headTailGameReuestDto.BetAmount,
                    Message = "Minimum balance required.Atleast $ 10 required ",
                    TotalAmount = totalAmount,
                    EntryDate = DateTime.Now,
                };
            }
            //headtailGame Logic 
            bool isWin = headTailGameReuestDto.BetSide;
                         isWin = false;
            decimal totalCreditAmount = await _userDbContext.Tab_FundTransaction
                              .Where(f => f.PId == headTailGameReuestDto.PId)
                              .SumAsync(f => (decimal?)f.CreditAmount) ?? 0;

            decimal totalDebitAmount = await _userDbContext.Tab_FundTransaction
                              .Where(f => f.PId == headTailGameReuestDto.PId)
                              .SumAsync(f => (decimal?)f.DebitAmount) ?? 0;
            if(totalDebitAmount > totalCreditAmount)
            {
                isWin=true;
            }
            else if(totalCreditAmount == totalDebitAmount)
            {
                isWin = true;

            }
            else if( totalCreditAmount > totalDebitAmount)
            {
                isWin = false;
            }
            decimal betAmount = headTailGameReuestDto.BetAmount;

            decimal resultAmount = isWin ? betAmount * 2 : betAmount;


            //Remark message to 
            string resultMessage = isWin ? $"Congratulations, You Win: {resultAmount}" : $"Sorry, You Lost: {resultAmount}";

            //After win/loss Current amount
            totalAmount = isWin ? totalAmount + resultAmount : totalAmount - resultAmount;

            //add new row to the transection table 
            var fundTransaction = new FundTransactionModel
            {
                GId = headTailGameReuestDto.GId,
                PId = headTailGameReuestDto.PId,
                CreditAmount = isWin ? resultAmount : 0,
                DebitAmount = isWin ? 0 : resultAmount,
                Remark = resultMessage,
                Type = isWin ? "Win" : "Loss",
                TransactionDate = DateTime.Now,
                TxNoId = $"TX_{new Random().Next(1000, 9999)}",
                Images = null
            };

            await _userDbContext.Tab_FundTransaction.AddAsync(fundTransaction);
            await _userDbContext.SaveChangesAsync();

            //add new row to the headtailgameIndes table 
            var newPlayGame = new HeadTailGameIndexModel
            {
                RandomId = $"HTGame_{new Random().Next(1000, 9999)}",
                PId = headTailGameReuestDto.PId,
                GId = headTailGameReuestDto.GId,
                BetAmount = headTailGameReuestDto.BetAmount,
                BetSide = headTailGameReuestDto.BetSide,
                Win = isWin ? resultAmount : 0,
                Loss = isWin ? 0:resultAmount,
                Type = isWin ? "win" : "loss",
                Remark = $"Game: {game.Name}, Player: {player.UserName} ",
                IsActive = true,
                EntryDate = DateTime.Now
            };

            await _userDbContext.Tab_HeadTailGameIndex.AddAsync(newPlayGame);
            await _userDbContext.SaveChangesAsync();



            // return the response to the user on swagger 
            var responseDto = new HeadTailGameResponseDto
            {
                PId = headTailGameReuestDto.PId,
                GId = headTailGameReuestDto.GId,
                Message = resultMessage,
                BetAmount = betAmount,
                EntryDate = DateTime.Now,
                TotalAmount = totalAmount

            };
            return responseDto;
        }


    }
}
