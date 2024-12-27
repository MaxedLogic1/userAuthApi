using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace usersAuthApi.Models.Domain
{
    public class BubbleGameModel
    {
        [Key]
        public int Id { get; set; }
        public int PId { get; set; }
        [ForeignKey("PId")]
        public UserModel User { get; set; }
        public decimal BetAmount { get; set; }
        public int Target { get; set; } = 10;
        public int AchiveTarget { get; set; }
        public string RandomId { get; set; }
        public bool IsWin { get; set; }
        public DateTime EntryDate { get; set; }
        public bool IsActive { get; set; }
    }
}
