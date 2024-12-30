using System.ComponentModel.DataAnnotations;

namespace usersAuthApi.Models.DTO
{
    public class HeadTailGameReuestDto
    {
        [Required]
        public int PId { get; set; }
        [Required] 
        public int  GId { get; set;}
        [Required]
        public decimal BetAmount { get; set; }
        [Required]
        public bool BetSide { get; set; }
        [Required]
        public DateTime BetEntryDate { get; set; }

    }
}
