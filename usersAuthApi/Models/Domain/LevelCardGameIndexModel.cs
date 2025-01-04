using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace usersAuthApi.Models.Domain
{
    public class LevelCardGameIndex
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("PId")]
        public int PId { get; set; }
        public UserModel? User { get; set; }
        [ForeignKey("GId")]
        public int GId { get; set; }
        public GamesModel? Games { get; set; }
        public decimal BetAmount { get; set; }
        //public bool BetSide { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Win { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Loss { get; set; }
        public string Type { get; set; }
        public string Remark { get; set; }
        public DateTime EntryDate { get; set; }
        public bool IsActive { get; set; }
        public string RandomId { get; set; }






    }
}
