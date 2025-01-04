using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using usersAuthApi.Models.Domain;

namespace usersAuthApi.Models.DTO
{
    public class FlappyBirdGameRequestDto
    {
        [Required]
        public int PId { get; set; }
        [Required]
        public int GId { get; set; }
        [Required]
        public decimal BetAmount { get; set; }
        [Required]
        public int TotalAchiveScore { get; set; }
        //[Required]
        //public DateTime TotalScoreTime { get; set; }
    }
}
