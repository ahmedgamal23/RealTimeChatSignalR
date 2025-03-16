namespace RealTimeChatSignalR.Models
{
    public class ChatMessage
    {
        public int Id { get; set; }
        public string SenderId { get; set; } = string.Empty;
        public ApplicationUser? Sender { get; set; }
        public string ReceiverId { get; set; } = string.Empty;
        public ApplicationUser? Receiver { get; set; }
        public string Message { get; set; } = string.Empty;
        public bool IsRead { get; set; } = false;
        public DateTime Timestamp { get; set; }
    }
}

