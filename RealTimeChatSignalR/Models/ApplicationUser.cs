using Microsoft.AspNetCore.Identity;

namespace RealTimeChatSignalR.Models
{
    public class ApplicationUser:IdentityUser
    {
        public string DisplayName { get; set; } = string.Empty;
        public byte[]? ImageUrl { get; set; }
        public virtual ICollection<Chat> Chats { get; set; } = default!;
    }
}
