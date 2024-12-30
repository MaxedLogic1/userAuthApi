namespace usersAuthApi.Models.DTO
{
    public class AllGameIndexRequestDto
    {
        public string GameName { get; set; }
        public decimal BetAmount { get; set; }
        public DateTime EntryDate { get; set; }
        public string WinLoss { get; set; }
    }
}
