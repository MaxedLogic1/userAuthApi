using System.ComponentModel.DataAnnotations;

namespace usersAuthApi.Models.Domain
{
    public class GamesModel
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public int Target { get; set; } = 0;
        public decimal Profit { get; set; } = 0;
        public decimal Percentage { get; set; } = 0;
        public string Description { get; set; }
        public string Type { get; set; }
        public string RandomId { get; set; }
        public DateTime EntryDate { get; set; }
        public bool IsActive { get; set; }
    }
}
