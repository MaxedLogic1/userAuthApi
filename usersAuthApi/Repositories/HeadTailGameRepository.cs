using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using usersAuthApi.ApplicationDbContext;
using usersAuthApi.Exceptions;
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
            //Assign time to current time 
            //headTailGameReuestDto.BetEntryDate = DateTime.Now.AddMinutes(330);

            //player is exsit
            var player = await _userDbContext.Tab_Register
              .Where(u => u.Id == headTailGameReuestDto.PId)
              .FirstOrDefaultAsync();
             

            //find the game 
            var game = await _userDbContext.Tab_Games
                .Where(g => g.Id == headTailGameReuestDto.GId)
                .FirstOrDefaultAsync();

            decimal totalCreditAmount = await _userDbContext.Tab_FundTransaction
                              .Where(f => f.PId == headTailGameReuestDto.PId)
                              .SumAsync(f => (decimal?)f.CreditAmount) ?? 0;

            decimal totalDebitAmount = await _userDbContext.Tab_FundTransaction
                              .Where(f => f.PId == headTailGameReuestDto.PId)
                              .SumAsync(f => (decimal?)f.DebitAmount) ?? 0;

            //Bet side from frontend show be true of false
            bool isWin = headTailGameReuestDto.BetSide;

            // rendomly win user 
            isWin = new Random().Next(100) % 2 == 0;
            if (isWin == false)
            {
                isWin = false;
            }
            else
            {
                isWin = true;
            }

            //if (totalDebitAmount > totalCreditAmount)
            //{
            //    isWin=true;
            //}
            //else if(totalCreditAmount == totalDebitAmount)
            //{
            //    isWin = true;

            //}
            //else if( totalCreditAmount > totalDebitAmount)
            //{
            //    isWin = false;
            //}

            //Bet Amout from user / frontend 
            decimal betAmount = headTailGameReuestDto.BetAmount;

            //Profit Come From GameTable
            //var profit = await _userDbContext.Tab_Games
            //     .Where(g => g.Id == bubbleGameRequestDto.GId)
            //     .Select(g => g.Profit)
            //     .FirstOrDefaultAsync();

            //Profit Percentage 
            var profitPercent = await _userDbContext.Tab_Games
               .Where(g => g.Id == headTailGameReuestDto.GId)
               .Select(g => g.Percentage)
               .FirstOrDefaultAsync();
            
            //assign result amount 
            decimal resultAmount = isWin ? betAmount * profitPercent : betAmount;

            //Remark message to 
            string resultMessage = isWin ? $"Congratulations, You Win: {resultAmount}" : $"Sorry, You Lost: {resultAmount}";
            string IsWin = isWin ? $"Win" : $"Loss";
          var  totalAmount = totalCreditAmount - totalDebitAmount;
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
                TransactionDate = DateTime.Now.AddMinutes(330),
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
                Loss = isWin ? 0 : resultAmount,
                Type = isWin ? "win" : "loss",
                Remark = $"Game: {game.Name}, Player: {player.UserName} ",
                IsActive = true,
                EntryDate = DateTime.Now.AddMinutes(330)
            };

            await _userDbContext.Tab_HeadTailGameIndex.AddAsync(newPlayGame);
            await _userDbContext.SaveChangesAsync();



            // return the response to the user on swagger 
            var responseDto = new HeadTailGameResponseDto
            {
                PId = headTailGameReuestDto.PId,
                PlayeName = player.Name,
                GId = headTailGameReuestDto.GId,
                GameName = game.Name,
                Message = IsWin,
                BetAmount = betAmount,
                EntryDate = DateTime.Now.AddMinutes(330),
                TotalAmount = totalAmount

            };
            return responseDto;
        }

        public async Task<List<HeadTailGameResponseDto>> MultiUserHeadTailGameResponse(HeadTailGameMultiUsersRequestDto headTailGameMultiUsersRequestDto)
        {
          //response Array to store all the user response 
            var responses = new List<HeadTailGameResponseDto>();

            //time to current time 
            var currentTime = DateTime.Now.AddMinutes(330);

            //sesstion start time 
            var sessionStartTime = new DateTime(currentTime.Year, currentTime.Month, currentTime.Day, currentTime.Hour, currentTime.Minute, 0);
           
            //sesstion end time 
            var sessionEndTime = sessionStartTime.AddMinutes(1).AddSeconds(-1);

            //loop for check each player to and save there details 
            foreach (var betRequest in headTailGameMultiUsersRequestDto.Bets)
            {
                //playe enter between session time
                if (betRequest.BetEntryDate > sessionStartTime && betRequest.BetEntryDate < sessionEndTime)
                {
                    // Player exists
                    var player = await _userDbContext.Tab_Register
                        .Where(u => u.Id == betRequest.PId)
                        .FirstOrDefaultAsync();
                    if (player == null)
                    {
                        //add response to response Array
                        responses.Add(new HeadTailGameResponseDto
                        {
                            PId = betRequest.PId,
                            GId = betRequest.GId,
                            Message = "Player not found.",
                            BetAmount = betRequest.BetAmount,
                            EntryDate = DateTime.Now.AddMinutes(330),

                        });
                        continue;
                    }

                    // Game exists
                    var game = await _userDbContext.Tab_Games
                        .Where(g => g.Id == betRequest.GId)
                        .FirstOrDefaultAsync();
                    if (game == null)
                    {
                        //add response to responses Array
                        responses.Add(new HeadTailGameResponseDto
                        {
                            PId = betRequest.PId,
                            PlayeName = player.Name,
                            GId = betRequest.GId,
                            Message = "Game not found.",
                            BetAmount = betRequest.BetAmount,
                            EntryDate = DateTime.Now.AddMinutes(330),
                        });
                        continue;
                    }

                    // Calculate the player's total balance
                    decimal totalAmount = await _userDbContext.Tab_FundTransaction
                        .Where(f => f.PId == betRequest.PId)
                        .GroupBy(f => 1)
                        .Select(g => (decimal?)(g.Sum(f => (decimal?)f.CreditAmount) ?? 0) - (g.Sum(f => (decimal?)f.DebitAmount) ?? 0))
                        .FirstOrDefaultAsync() ?? 0;

                    if (totalAmount <= betRequest.BetAmount)
                    {
                        //Player Balance is less the betAmount
                        responses.Add(new HeadTailGameResponseDto
                        {
                            PId = betRequest.PId,
                            PlayeName = player.Name,
                            GId = betRequest.GId,
                            GameName = game.Name,
                            Message = "Minimum balance required. At least $10 required.",
                            BetAmount = betRequest.BetAmount,
                            EntryDate = DateTime.Now.AddMinutes(330),
                            TotalAmount = totalAmount
                        });
                        continue;
                    }

                    // Simulate win/loss
                    bool isWin = new Random().Next(100) % 2 == 0;

                    // Define win amount (20% of the bet)
                    decimal resultAmount = isWin ? betRequest.BetAmount * (decimal)0.20 : betRequest.BetAmount;

                    // Prepare the transaction message
                    string resultMessage = isWin ? $"Congratulations, You Win: {resultAmount}" : $"Sorry, You Lost: {resultAmount}";

                    // Update the total amount after win/loss
                    totalAmount = isWin ? totalAmount + resultAmount : totalAmount - resultAmount;

                    // Add transaction to the database
                    var fundTransaction = new FundTransactionModel
                    {
                        GId = betRequest.GId,
                        PId = betRequest.PId,
                        CreditAmount = isWin ? resultAmount : 0,
                        DebitAmount = isWin ? 0 : resultAmount,
                       
                        Remark = resultMessage,
                        Type = isWin ? "Win" : "Loss",
                        TransactionDate = DateTime.Now.AddMinutes(330),
                        TxNoId = $"TX_{new Random().Next(1000, 9999)}",
                        Images = null
                    };

                    await _userDbContext.Tab_FundTransaction.AddAsync(fundTransaction);

                    // Record the game index
                    var newPlayGame = new HeadTailGameIndexModel
                    {
                        RandomId = $"HTGame_{new Random().Next(1000, 9999)}",
                        PId = betRequest.PId,
                        GId = betRequest.GId,
                        BetAmount = betRequest.BetAmount,
                        BetSide = isWin ? true : false,
                        Win = isWin ? resultAmount : 0,
                        Loss = isWin ? 0 : resultAmount,
                        Type = isWin ? "Win" : "Loss",
                        Remark = $"Game: {game.Name}, Player: {player.UserName}",
                        IsActive = true,
                        EntryDate = DateTime.Now.AddMinutes(330)
                    };

                    await _userDbContext.Tab_HeadTailGameIndex.AddAsync(newPlayGame);

                    await _userDbContext.SaveChangesAsync();

                    // Prepare the response
                    var responseDto = new HeadTailGameResponseDto
                    {
                        PId = betRequest.PId,
                        PlayeName = player.Name,
                        GId = betRequest.GId,
                        GameName = game.Name,
                        Message = resultMessage,
                        BetAmount = betRequest.BetAmount,
                        EntryDate = DateTime.Now.AddMinutes(330),
                        TotalAmount = totalAmount
                    };

                    responses.Add(responseDto);
                }
                else
                {
                    //player not entred in the given session time
                    responses.Add(new HeadTailGameResponseDto
                    {
                        Message = "Session timeout: Bet placed outside the allowed 1 minute session.",
                        BetAmount = betRequest.BetAmount,
                        EntryDate = betRequest.BetEntryDate
                    });
                }
               
            }
            return responses;
        }
    }
}
