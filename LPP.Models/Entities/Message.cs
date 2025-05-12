namespace LPP.Models.Entities
{
    public class Message
    {
        public int Id { get; set; }

        public string Text { get; set; } = string.Empty;

        public string Title { get; set; } = string.Empty;

        public int TelegramMessageId { get; set; }

        public MessageType MessageType { get; set; } = MessageType.None;

        public bool IsDeleted { get; set; } = false;
    }
}
