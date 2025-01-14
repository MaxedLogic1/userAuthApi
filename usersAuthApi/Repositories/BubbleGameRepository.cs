﻿using System.Numerics;
using Microsoft.EntityFrameworkCore;
using usersAuthApi.ApplicationDbContext;
using usersAuthApi.Exceptions;
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
           
            //player is exsit
            var player = await _userDbContext.Tab_Register
              .Where(u => u.Id == bubbleGameRequestDto.PId)
              .FirstOrDefaultAsync();

            //find the game 
            var game = await _userDbContext.Tab_Games
                .Where(g => g.Id == bubbleGameRequestDto.GId)
                .FirstOrDefaultAsync();

            //Player totalCreditAmount 
            decimal creditAmount = await _userDbContext.Tab_FundTransaction
                .Where(f => f.PId == bubbleGameRequestDto.PId)
                .SumAsync(f => (decimal?)f.CreditAmount) ?? 0;

            //Player totalCreditAmount 
            decimal debitAmount = await _userDbContext.Tab_FundTransaction
               .Where(f => f.PId == bubbleGameRequestDto.PId)
               .SumAsync(f => (decimal?)f.DebitAmount) ?? 0;

            //Target Come from DataBase
            var target = await _userDbContext.Tab_Games
                  .Where(g => g.Id == bubbleGameRequestDto.GId)
                  .Select(g => g.Target)   
                  .FirstOrDefaultAsync();

            //Profit Come From GameTable
            //var profit = await _userDbContext.Tab_Games
            //     .Where(g => g.Id == bubbleGameRequestDto.GId)
            //     .Select(g => g.Profit)
            //     .FirstOrDefaultAsync();

            //Profit Percentage 
            var profitPercent = await _userDbContext.Tab_Games
               .Where(g => g.Id == bubbleGameRequestDto.GId)
               .Select(g => g.Percentage)
               .FirstOrDefaultAsync();

            //assign betAmount
            decimal betAmount = bubbleGameRequestDto.BetAmount;

            //Achive target is greter than target
            bool isWin = bubbleGameRequestDto.AchiveTarget >= target;

            //Decide result base on Achivet target
            decimal resultAmount = isWin ? betAmount * profitPercent : betAmount;

            //Set messaget to save in the remark section 
            string resultMessage = isWin ? $"Congratulations, You Win: {resultAmount}" : $"Sorry, You Lost: {resultAmount}";
            string IsWin = isWin ? $"Win" : $"Loss";
            //Total Amount 
            decimal currentAmount = creditAmount - debitAmount;

            //After win/loss Current amount
            currentAmount = isWin ? currentAmount + resultAmount : currentAmount - resultAmount;

            //Add New Row in the tab_fund table 
            var fundTransaction = new FundTransactionModel
            {
                GId = bubbleGameRequestDto.GId,
                PId = bubbleGameRequestDto.PId,
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
            var newPlayGame = new BubbleGameIndexModel
            {
                RandomId = $"BGame_{new Random().Next(1000, 9999)}",
                PId = bubbleGameRequestDto.PId,
                GId = bubbleGameRequestDto.GId,
                BetAmount = bubbleGameRequestDto.BetAmount,
                Type = isWin ? "Win" : "Loss",
                Win = isWin ? resultAmount : 0,
                Loss = isWin ? 0 : resultAmount,
                Remark = $"Game: {game.Name}, Player: {player.UserName} ",
                IsActive = true,
                EntryDate = DateTime.Now.AddMinutes(330)
            };

            //save new row in Tab_BubbleGameIndex table 
            await _userDbContext.Tab_BubbleGameIndex.AddAsync(newPlayGame);
            await _userDbContext.SaveChangesAsync();

            //return to the user and you want to remove this
            var responseDto = new BubbleGameResponseDto
            {
                PId = bubbleGameRequestDto.PId,
                GId = bubbleGameRequestDto.GId,
                Message = IsWin,
                BetAmount = betAmount,
                EntryDate = DateTime.Now.AddMinutes(330),
                TotalAmount = currentAmount

            };
            // return user 
            return responseDto;
        }
           
        
    }
}
