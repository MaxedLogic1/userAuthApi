using Microsoft.EntityFrameworkCore;
using usersAuthApi.Models.Domain;

namespace usersAuthApi.ApplicationDbContext
{
    public class userDbContext : DbContext
    {
        public userDbContext(DbContextOptions<userDbContext> options) : base(options)
        {

        }
        public DbSet<UserModel> Tab_Register { get; set; }
        public DbSet<FundTransactionModel> Tab_FundTransaction { get; set; }
        public DbSet<GamesModel> Tab_Games { get; set; }
        public DbSet<BubbleGameIndexModel> Tab_BubbleGameIndex { get; set; }
        public DbSet<BubbleGameModel> Tab_BubbleGame { get; set; }
        public DbSet<HeadTailGameIndexModel> Tab_HeadTailGameIndex {  get; set; }

    }
}
