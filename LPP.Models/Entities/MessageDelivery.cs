namespace LPP.Models.Entities
{
    public class MessageDelivery 
    {
        public int Id { get; set; }

        public int MessageId { get; set; }

        public int UserId { get; set; }

        public long ChatId { get; set; }

        public int TelegramMessageId { get; set; }
    }
}
