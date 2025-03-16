namespace RealTimeChatSignalR.Models
{
    public class ChatUser
    {
        public int Id { get; set; }

        public string UserId { get; set; } = string.Empty;
        public virtual ApplicationUser? User { get; set; }

        public int ChatId { get; set; }
        public virtual Chat? Chat { get; set; }
    }
}
