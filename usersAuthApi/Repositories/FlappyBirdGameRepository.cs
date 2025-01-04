using Microsoft.EntityFrameworkCore;
using usersAuthApi.ApplicationDbContext;
using usersAuthApi.Models.Domain;
using usersAuthApi.Models.DTO;

namespace usersAuthApi.Repositories
{
    public class FlappyBirdGameRepository : IFlappyBirdGameRepository
    {
        private readonly userDbContext _userDbContext;

        public FlappyBirdGameRepository(userDbContext userDbContext)
        {
            _userDbContext = userDbContext;
        }
        // This Method base on the Socre 
        public async Task<FlappyBirdGameResponseDto> FlappyBirdGameResponse(FlappyBirdGameRequestDto flappyBirdGameRequestDto)
        {

            //player is exsit
            var player = await _userDbContext.Tab_Register
              .Where(u => u.Id == flappyBirdGameRequestDto.PId)
              .FirstOrDefaultAsync();

            //find the game 
            var game = await _userDbContext.Tab_Games
                .Where(g => g.Id == flappyBirdGameRequestDto.GId)
                .FirstOrDefaultAsync();

            //Player totalCreditAmount 
            decimal creditAmount = await _userDbContext.Tab_FundTransaction
                .Where(f => f.PId == flappyBirdGameRequestDto.PId)
                .SumAsync(f => (decimal?)f.CreditAmount) ?? 0;

            //Player totalCreditAmount 
            decimal debitAmount = await _userDbContext.Tab_FundTransaction
               .Where(f => f.PId == flappyBirdGameRequestDto.PId)
               .SumAsync(f => (decimal?)f.DebitAmount) ?? 0;

            
            //Profit Come From GameTable
            //var profit = await _userDbContext.Tab_Games
            //     .Where(g => g.Id == flappyBirdGameRequestDto.GId)
            //     .Select(g => g.Profit)
            //     .FirstOrDefaultAsync();

            //Profit Percentage 
            //var profitPercent = await _userDbContext.Tab_Games
            //   .Where(g => g.Id == flappyBirdGameRequestDto.GId)
            //   .Select(g => g.Percentage)
            //   .FirstOrDefaultAsync();

            //FlappyBirdGame logic TotalAchiveScore 

            //assign betAmount
            decimal betAmount = flappyBirdGameRequestDto.BetAmount;

            bool isWin = false;
            decimal resultAmount = (decimal)0.0; // Start with the original bet amount

            // Check the TotalAchiveScore to decide if the player won or lost
            if (flappyBirdGameRequestDto.TotalAchiveScore <= 0)
            {
                isWin = false; // user hit the column, loss
                resultAmount = betAmount;
            }
            else if (flappyBirdGameRequestDto.TotalAchiveScore >= 10 && flappyBirdGameRequestDto.TotalAchiveScore <= 20)
            {
                isWin = true;
                resultAmount = betAmount * (decimal)0.2; // 2% profit
            }
            else if (flappyBirdGameRequestDto.TotalAchiveScore >= 20 && flappyBirdGameRequestDto.TotalAchiveScore <= 30)
            {
                isWin = true;
                resultAmount = betAmount * (decimal)0.4; // 4% profit
            }
            else if (flappyBirdGameRequestDto.TotalAchiveScore >= 30 && flappyBirdGameRequestDto.TotalAchiveScore <= 40)
            {
                isWin = true;
                resultAmount = betAmount * (decimal)0.6; // 6% profit
            }
            else if (flappyBirdGameRequestDto.TotalAchiveScore >= 40 && flappyBirdGameRequestDto.TotalAchiveScore <= 50)
            {
                isWin = true;
                resultAmount = betAmount * (decimal)0.8; // 8% profit
            }
            else if (flappyBirdGameRequestDto.TotalAchiveScore >= 50)
            {
                isWin = true;
                resultAmount = betAmount * (decimal)1.0; // 10% profit
            }

            // Set message to save in the remark section
            string checkWinLoss = isWin ? $"Win" :$"Loss";
            string resultMessage = isWin ? $"Congratulations, You Win: {resultAmount}" : $"Sorry, You Lost: {resultAmount}";

            //Total Amount 
            decimal currentAmount = creditAmount - debitAmount;

            //After win/loss Current amount
            currentAmount = isWin ? currentAmount + resultAmount : currentAmount - resultAmount;

            //Add New Row in the tab_fund table 
            var fundTransaction = new FundTransactionModel
            {
                GId = flappyBirdGameRequestDto.GId,
                PId = flappyBirdGameRequestDto.PId,
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
            var newPlayGame = new FlappyBirdGameIndexModel
            {
                PId = flappyBirdGameRequestDto.PId,
                GId = flappyBirdGameRequestDto.GId,
                BetAmount = flappyBirdGameRequestDto.BetAmount,
                Type = isWin ? "Win" : "Loss",
                TotalAchiveScore = flappyBirdGameRequestDto.TotalAchiveScore,
               // TotalScoreTime = TimeOnly.Fro
                Win = isWin ? resultAmount : 0,
                Loss = isWin ? 0 : resultAmount,
                Remark = $"Game: {game.Name}, Player: {player.UserName} ",
                IsActive = true,
                RandomId = $"LCGame_{new Random().Next(1000, 9999)}",
                EntryDate = DateTime.Now.AddMinutes(330)
            };

            //save new row in Tab_BubbleGameIndex table 
            await _userDbContext.Tab_FlappyBirdGameIndex.AddAsync(newPlayGame);
            await _userDbContext.SaveChangesAsync();

            //return to the user and you want to remove this
            var responseDto = new FlappyBirdGameResponseDto
            {
                PId = flappyBirdGameRequestDto.PId,
                GId = flappyBirdGameRequestDto.GId,
                Message = checkWinLoss,
                BetAmount = betAmount,
                EntryDate = DateTime.Now.AddMinutes(330),
                TotalAmount = currentAmount

            };
            // return user 
            return responseDto;
        }

        //This Method Base on the Time 
        public async Task<FlappyBirdGameResponseDto> FlappyBirdGameResponseTime(FlappyBirdGameTimeRequestDto flappyBirdTimeGameRequestDto)
        {

            //player is exsit
            var player = await _userDbContext.Tab_Register
              .Where(u => u.Id == flappyBirdTimeGameRequestDto.PId)
              .FirstOrDefaultAsync();

            //find the game 
            var game = await _userDbContext.Tab_Games
                .Where(g => g.Id == flappyBirdTimeGameRequestDto.GId)
                .FirstOrDefaultAsync();

            //Player totalCreditAmount 
            decimal creditAmount = await _userDbContext.Tab_FundTransaction
                .Where(f => f.PId == flappyBirdTimeGameRequestDto.PId)
                .SumAsync(f => (decimal?)f.CreditAmount) ?? 0;

            //Player totalCreditAmount 
            decimal debitAmount = await _userDbContext.Tab_FundTransaction
               .Where(f => f.PId == flappyBirdTimeGameRequestDto.PId)
               .SumAsync(f => (decimal?)f.DebitAmount) ?? 0;


            //Profit Come From GameTable
            //var profit = await _userDbContext.Tab_Games
            //     .Where(g => g.Id == flappyBirdGameRequestDto.GId)
            //     .Select(g => g.Profit)
            //     .FirstOrDefaultAsync();

            //Profit Percentage 
            //var profitPercent = await _userDbContext.Tab_Games
            //   .Where(g => g.Id == flappyBirdGameRequestDto.GId)
            //   .Select(g => g.Percentage)
            //   .FirstOrDefaultAsync();

            //FlappyBirdGame logic TotalAchiveScore 

            //assign betAmount
            decimal betAmount = flappyBirdTimeGameRequestDto.BetAmount;


           TimeOnly startGameTime = flappyBirdTimeGameRequestDto.StartGameTime.AddMinutes(330);
            TimeOnly endGameTime = flappyBirdTimeGameRequestDto.EndGameTime.AddMinutes(330);

            // Calculate the time difference (TimeSpan)
            TimeSpan totalScoreTime = endGameTime - startGameTime;

            // If you want to calculate the end time from the start time by adding the duration:
            TimeOnly totalPlayedTime = TimeOnly.MinValue.Add(totalScoreTime);


            bool isWin = false;
            decimal resultAmount = (decimal)0.0; // Start with the original bet amount

            // Check the TotalAchiveScore to decide if the player won or lost
            if (totalScoreTime <= TimeSpan.FromMilliseconds(3))
            {
                isWin = false; // user hit the column, loss
                resultAmount = betAmount;
            }
            else if (totalScoreTime >= TimeSpan.FromMilliseconds(3) && totalScoreTime >= TimeSpan.FromMilliseconds(59))
            {
                isWin = true;
                resultAmount = betAmount * (decimal)0.2; // 2% profit
            }
            else if (totalScoreTime >= TimeSpan.FromMinutes(60) && totalScoreTime >= TimeSpan.FromMinutes(120))
            {
                isWin = true;
                resultAmount = betAmount * (decimal)0.4; // 4% profit
            }
            

            // Set message to save in the remark section
            string checkWinLoss = isWin ? $"Win" : $"Loss";
            string resultMessage = isWin ? $"Congratulations, You Win: {resultAmount}" : $"Sorry, You Lost: {resultAmount}";

            //Total Amount 
            decimal currentAmount = creditAmount - debitAmount;

            //After win/loss Current amount
            currentAmount = isWin ? currentAmount + resultAmount : currentAmount - resultAmount;

            //Add New Row in the tab_fund table 
            var fundTransaction = new FundTransactionModel
            {
                GId = flappyBirdTimeGameRequestDto.GId,
                PId = flappyBirdTimeGameRequestDto.PId,
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
            var newPlayGame = new FlappyBirdGameIndexModel
            {
                PId = flappyBirdTimeGameRequestDto.PId,
                GId = flappyBirdTimeGameRequestDto.GId,
                BetAmount = flappyBirdTimeGameRequestDto.BetAmount,
                Type = isWin ? "Win" : "Loss",
                TotalAchiveScore = 0,
                TotalScoreTime = totalPlayedTime,
                Win = isWin ? resultAmount : 0,
                Loss = isWin ? 0 : resultAmount,
                Remark = $"Game: {game.Name}, Player: {player.UserName} ",
                IsActive = true,
                RandomId = $"LCGame_{new Random().Next(1000, 9999)}",
                EntryDate = DateTime.Now.AddMinutes(330)
            };

            //save new row in Tab_BubbleGameIndex table 
            await _userDbContext.Tab_FlappyBirdGameIndex.AddAsync(newPlayGame);
            await _userDbContext.SaveChangesAsync();

            //return to the user and you want to remove this
            var responseDto = new FlappyBirdGameResponseDto
            {
                PId = flappyBirdTimeGameRequestDto.PId,
                GId = flappyBirdTimeGameRequestDto.GId,
                Message = checkWinLoss,
                BetAmount = betAmount,
                EntryDate = DateTime.Now.AddMinutes(330),
                TotalAmount = currentAmount

            };
            // return user 
            return responseDto;
        }
 

    }
}
