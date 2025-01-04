//using Microsoft.EntityFrameworkCore;
//using usersAuthApi.ApplicationDbContext;
//using usersAuthApi.Models.Domain;
//using usersAuthApi.Models.DTO;

//namespace usersAuthApi.Repositories
//{
//    public class PlayedGameRepository : IPlayedGameRepository
//    {
//        private readonly userDbContext _userDbContext;

//        public PlayedGameRepository(userDbContext userDbContext)
//        {
//            _userDbContext = userDbContext;
//        }

//        public async Task<PlayedGameResponseDto> AddPlayGameAsync(PlayedGameRequestDto playGameRequestDto)
//        {

//            //Find That Plyer 
//            var player = await _userDbContext.Tab_Register
//                .Where(u => u.Id == playGameRequestDto.PId)
//                .FirstOrDefaultAsync();
//            //check player is not null ya null
//            if (player == null)
//            {
//                throw new InvalidOperationException("Player not found.");
//            }

//            //find the game 
//            var game = await _userDbContext.Tab_Games
//                .Where(g => g.Id == playGameRequestDto.GId)
//                .FirstOrDefaultAsync();
//            //check the game is null or not 
//            if (game == null)
//            {
//                throw new InvalidOperationException("Game not found.");
//            }

//            //find the total amount using PId 
//            decimal creditAmount = await _userDbContext.Tab_FundTransaction
//                    .Where(f => f.PId == playGameRequestDto.PId)
//                    .SumAsync(f => (decimal?)f.CreditAmount) ?? 0;

//            decimal debitAmount = await _userDbContext.Tab_FundTransaction
//               .Where(f => f.PId == playGameRequestDto.PId)
//               .SumAsync(f => (decimal?)f.DebitAmount) ?? 0;

//            decimal TotalAmount = creditAmount - debitAmount;

//            var newPlayGame = new BubbleGameIndexModel
//            {
//                RandomId = $"PGame_{new Random().Next(1000, 9999)}",
//                PId = playGameRequestDto.PId, 
//                GId = playGameRequestDto.GId, 
//               // BetAmount = playGameRequestDto.BetAmount,
//                WinLoss = playGameRequestDto.WinLoss,  
//                Remark = $"Your Game Id: {playGameRequestDto.GId}, Your Player Id: {playGameRequestDto.PId}, Game: {game.Name}, Player: {player.UserName}", 
//                IsActive = true, 
//                EntryDate = DateTime.Now.AddMinutes(330) 
//            };

//           await _userDbContext.Tab_BubbleGameIndex.AddAsync(newPlayGame);
//            await _userDbContext.SaveChangesAsync();  

//            var responseDto = new PlayedGameResponseDto
//            {
//                Id = newPlayGame.Id,
//                PlayerName = player.UserName,  
//                GameName = game.Name, 
//                BetAmount = newPlayGame.BetAmount,
//                EntryDate = newPlayGame.EntryDate,
//                WinLoss = newPlayGame.WinLoss,
//                TotalAmount = TotalAmount,


//            };

//            // Return the response DTO
//            return responseDto;
//        }

//    }
//}
