# RealTimeChatSignalR 🚀

![RealTime Chat](assets/chat_demo.png)  
*A real-time chat application using ASP.NET Core and SignalR.*

## 📌 Overview
**RealTimeChatSignalR** is a modern web-based chat application that allows users to communicate instantly using **ASP.NET Core** and **SignalR**. This project demonstrates how to implement real-time messaging in a scalable and efficient way.

## 🎯 Features
- 🔹 **Real-time messaging** using SignalR WebSockets
- 🔹 **User authentication & authorization** with ASP.NET Identity
- 🔹 **Multiple chat rooms** for group discussions
- 🔹 **Online user status** tracking
- 🔹 **Message history storage** with Entity Framework Core
- 🔹 **Responsive UI** with Bootstrap & JavaScript

## 🏗️ Tech Stack
- **Backend:** ASP.NET Core 9, SignalR, Entity Framework Core
- **Frontend:** HTML, CSS, JavaScript, Bootstrap
- **Database:** SQL Server
- **Authentication:** ASP.NET Identity

## 📡 What is SignalR?
- is a real-time communication library in ASP.NET that enables **bi-directional communication** between clients and servers. It supports:

- **WebSockets** (preferred for performance)
- **Server-Sent Events (SSE)**
- **Long Polling** (fallback when WebSockets are unavailable)

### 🔥 How SignalR Works
1. A **SignalR Hub** is created on the server.
2. Clients connect to the hub using JavaScript.
3. Messages are sent and received in real-time without page reloads.

![SignalR Workflow](assets/signalr_workflow.png)

## 🚀 Getting Started
### 1️⃣ Clone the Repository
```sh
git clone https://github.com/yourusername/RealTimeChatSignalR.git
cd RealTimeChatSignalR
```

### 2️⃣ Setup Database
- Open `appsettings.json` and configure your **SQL Server connection string**.
- Run migrations:
```sh
dotnet ef database update
```

### 3️⃣ Run the Application
```sh
dotnet run
```
Navigate to `http://localhost:5000` in your browser.

## ⚙️ SignalR Hub Example
```csharp
public class ChatHub : Hub
{
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
}
```

