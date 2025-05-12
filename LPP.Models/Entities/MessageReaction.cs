namespace LPP.Models.Entities
{
    public class MessageReaction
    {
        public int Id { get; set; }

        public int MessageId { get; set; } = 0;

        public int UserId { get; set; }

        public MessageReactionType MessageReactionType { get; set; }
    }
}
