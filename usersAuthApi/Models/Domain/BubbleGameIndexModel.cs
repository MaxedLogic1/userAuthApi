using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace usersAuthApi.Models.Domain
{
    public class BubbleGameIndexModel
    {
        [Key]
        public int Id { get; set; }
        
        [Column(TypeName = "money")]
        public decimal BetAmount { get; set; }
        public string RandomId { get; set; }
       // public int Target { get; set; }
       // public int AchiveTarget { get; set; }
        public string WinLoss { get; set; }

        [StringLength(500, ErrorMessage = "Remark cannot exceed 500 characters.")]
       public string Remark { get; set; }
        public DateTime EntryDate { get; set; }
        public bool IsActive { get; set; }

        public int GId { get; set; }
        [ForeignKey("GId")]
        public GamesModel Game { get; set; }

        public int PId { get; set; }
        [ForeignKey("PId")]
        public UserModel User { get; set; }

    }
}
