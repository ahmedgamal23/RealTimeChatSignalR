using RealTimeChatSignalR.Hubs;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RealTimeChatSignalR.Data;
using RealTimeChatSignalR.Models;

namespace RealTimeChatSignalR
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var connectionString = builder.Configuration.GetConnectionString("RealTimeChatSignalRContextConnection") ?? throw new InvalidOperationException("Connection string 'RealTimeChatSignalRContextConnection' not found.");;

            builder.Services.AddDbContext<RealTimeChatSignalRContext>(options => options.UseSqlServer(connectionString));

            builder.Services.AddDefaultIdentity<ApplicationUser>(
                options =>
                {
                    options.SignIn.RequireConfirmedAccount = true;
                    options.Password.RequireDigit = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequiredLength = 5;
                }
            ).AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<RealTimeChatSignalRContext>();

            builder.Services.AddSession(option =>
            {
                option.IdleTimeout = TimeSpan.FromSeconds(5);
                option.Cookie.HttpOnly = true;
                option.Cookie.IsEssential = true;
                option.Cookie.Name = ".RealTimeChatSignalR.Session";
            });

            builder.Services.AddAuthentication(option => {
                option.DefaultAuthenticateScheme = IdentityConstants.ApplicationScheme;
                option.DefaultSignInScheme = IdentityConstants.ApplicationScheme;
                option.DefaultChallengeScheme = IdentityConstants.ApplicationScheme;
            });

            // Add services
            builder.Services.AddRazorPages();
            builder.Services.AddControllersWithViews();
            builder.Services.AddSignalR();

            var app = builder.Build();

            // Handle errors
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseSession();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            // Map routes
            app.MapControllerRoute(
                name:"area",
                pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
            );
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}"
            );

            // Register SignalR hub
            app.MapHub<ChatHub>("/chatHub");

            app.MapRazorPages();

            app.Run();
        }
    }
}
