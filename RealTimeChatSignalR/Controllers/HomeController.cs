using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using RealTimeChatSignalR.Models;
using RealTimeChatSignalR.Hubs;
using Microsoft.AspNetCore.Authorization;
using RealTimeChatSignalR.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace RealTimeChatSignalR.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly RealTimeChatSignalRContext _context;
        private SignInManager<ApplicationUser> _signInManager;

        public HomeController(ILogger<HomeController> logger, RealTimeChatSignalRContext context, SignInManager<ApplicationUser> signInManager)
        {
            _logger = logger;
            _context = context;
            _signInManager = signInManager;
        }

        [Authorize]
        public IActionResult Index()
        {
            var curUser = _signInManager.UserManager.GetUserAsync(User).Result;
            var users = _context.Users.Where(x => x.Id != curUser!.Id);
            
            ViewData["StatusMessage"] = HttpContext.Session.GetString("StatusMessage");
            return View(users);
        }

        [HttpGet]
        public async Task<IActionResult> GetMessages(string userId)
        {
            var user = await _signInManager.UserManager.GetUserAsync(User);

            // Get messages
            var messages = await _context.ChatMessages
                .Include(m => m.Sender)
                .Include(m => m.Receiver)
                .Where(m => (m.SenderId == userId && m.ReceiverId == user!.Id) ||
                            (m.SenderId == user!.Id && m.ReceiverId == userId))
                .OrderBy(m => m.Timestamp)
                .Select(m => new { m.Id, m.SenderId, SenderName = m.Sender!.DisplayName, m.Message, m.Timestamp, m.IsRead })
                .ToListAsync();

            // Mark messages as read
            var unreadMessages = _context.ChatMessages
                .Where(m => m.SenderId == userId && m.ReceiverId == user!.Id && !m.IsRead)
                .ToList();

            if (unreadMessages.Any())
            {
                foreach (var msg in unreadMessages)
                {
                    msg.IsRead = true;
                }
                await _context.SaveChangesAsync();
            }

            return Json(messages);
        }


        [HttpGet]
        public async Task<IActionResult> GetUnReadMessages()
        {
            var user = await _signInManager.UserManager.GetUserAsync(User);

            var unReadMessages = await _context.ChatMessages
                .Where(m => m.ReceiverId == user!.Id && !m.IsRead)
                .GroupBy(m => m.SenderId)
                .Select(g => new { SenderId = g.Key, UnreadCount = g.Count() })
                .ToListAsync();

            return Json(unReadMessages);
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
