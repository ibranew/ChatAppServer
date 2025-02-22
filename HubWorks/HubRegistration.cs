using ChatServerExample.HubWorks.Hubs;

namespace ChatServerExample.HubWorks
{
    public static class HubRegistration
    {
        public static void AddChatServerExampleMapHubs(this WebApplication webApplication)
        {
            webApplication.MapHub<ChatHub>("/chat-hub");
        }
    }
}
