namespace RealTimeChatSignalR.Models
{
    public class Message
    {
        public int Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public string? ImageUrl { get; set; } // if it’s an image message
        public DateTime SendAt { get; set; } = DateTime.Now;
        public bool IsRead { get; set; } = false; // Flag to check if the message is read

        public string SenderId { get; set; } = string.Empty; // FK for the sender
        public virtual ApplicationUser? Sender { get; set; } // navigation property for the sender

        public int ChatId { get; set; } // FK for the chat
        public virtual Chat? Chat { get; set; } // navigation property for the chat
    }
}
