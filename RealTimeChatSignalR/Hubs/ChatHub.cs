using Microsoft.AspNetCore.SignalR;
using RealTimeChatSignalR.Data;
using RealTimeChatSignalR.Models;
using System.Collections.Concurrent;

namespace RealTimeChatSignalR.Hubs
{
    public class ChatHub : Hub
    {
        // store the connection id of the user
        private static ConcurrentDictionary<string, string> _connections = new ConcurrentDictionary<string, string>();
        private readonly RealTimeChatSignalRContext _context;

        public ChatHub(RealTimeChatSignalRContext context)
        {
            _context = context;
        }

        public override Task OnConnectedAsync()
        {
            var userId = Context.UserIdentifier;
            _connections[userId!] = Context.ConnectionId;
            return base.OnConnectedAsync();
        }

        public async Task SendMessageToAll(string message, string ConnectionId)
        {
            await Clients.All.SendAsync("ReceiveMessage", message);
        }

        public async Task SendMessageToUser(string receiverId, string message)
        {
            var senderId = Context.UserIdentifier;

            // save the message to the database
            var chatMessage = new ChatMessage
            {
                SenderId = senderId!,
                ReceiverId = receiverId,
                Message = message,
                Timestamp = DateTime.Now,
                IsRead = false
            };

            await _context.ChatMessages.AddAsync(chatMessage);
            await _context.SaveChangesAsync();

            if (_connections.TryGetValue(receiverId, out string? receiverConnectionId))
            {
                await Clients.Client(receiverConnectionId).SendAsync("ReceiveMessage", senderId, message);
            }
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            var userId = Context.UserIdentifier;
            _connections.TryRemove(userId!, out _);
            return base.OnDisconnectedAsync(exception);
        }
    }
}
