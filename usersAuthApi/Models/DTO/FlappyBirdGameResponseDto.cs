using System.ComponentModel.DataAnnotations.Schema;
using usersAuthApi.Models.Domain;

namespace usersAuthApi.Models.DTO
{
    public class FlappyBirdGameResponseDto
    {

        public int PId { get; set; }
        public int GId { get; set; }
        public decimal BetAmount { get; set; }
        public int TotalAchiveScore { get; set; }
        public DateTime TotalScoreTime { get; set; }
        public DateTime EntryDate { get; set; }
        public string Message { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
