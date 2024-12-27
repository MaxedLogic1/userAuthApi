namespace usersAuthApi.Models.DTO
{
    public class gameResponseDto
    {
        public int Id { get; set; }
        public string RandomId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public DateTime EntryDate { get; set; }
        public bool IsActive { get; set; }
        public int Target { get; set; }
        public decimal Profit { get; set; }
        public decimal Percentage { get; set; }



    }
}
