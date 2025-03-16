namespace RealTimeChatSignalR.Models
{
    public class Chat
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;  // Name of the chat
        public bool IsGroup { get; set; } // Flag to check if it’s a group chat

        public virtual ICollection<ApplicationUser> Users { get; set; } = default!; // Users in the chat
        public virtual ICollection<Message> Messages { get; set; } = default!; // Messages in the chat
    }
}
