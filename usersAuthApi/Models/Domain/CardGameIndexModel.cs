using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace usersAuthApi.Models.Domain
{
    public class CardGameIndexModel
    {
        [Key]
        public int Id { get; set; }

        [Column(TypeName = "money")]
        public decimal BetAmount { get; set; }
        public bool IsMatched { get; set; }
        public string Symbol { get; set; } //"A","B" 1,2 
        public string RandomId { get; set; }
        public string WinLoss { get; set; }
        public string Remark { get; set; }
        public DateTime EntryDate { get; set; }
        public bool IsActive { get; set; }
        public int GId { get; set; }
        [ForeignKey("GId")]
        public GamesModel? Game { get; set; }
        public int PId { get; set; }
        [ForeignKey("PId")]
        public UserModel? User { get; set; }

    }
}
