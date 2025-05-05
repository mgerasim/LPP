namespace LPP.Models.Entities
{
    public class User
    {
        public int Id { get; set; }
        public long TelegramId { get; set; }
        public string? Username { get; set; }
        public string? LastName { get; set; }
        public string? FirstName { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsAdmin { get; set; } = false;
    }
}
