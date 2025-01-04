namespace usersAuthApi.Models.DTO
{
    public class BubbleGameIndexResponseDto
    {
        public string GameName { get; set; }
        public decimal BetAmount { get; set; }
        public DateTime EntryDate { get; set; }
        //type win/loss 
        public string Type { get; set; }
    }
}
