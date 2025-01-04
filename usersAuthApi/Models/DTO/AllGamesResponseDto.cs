namespace usersAuthApi.Models.DTO
{
    public class AllGamesResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public DateTime EntryDate { get; set; }
        public bool IsActive { get; set; }
    }
}
