using ChatServerExample.HubWorks.Hubs;
using ChatServerExample.HubWorks.Services.Concretes;

namespace ChatServerExample.HubWorks
{
    public static class ServiceRegistration
    {
        public static void AddChatServerExampleServices(this IServiceCollection collection)
        {
            collection.AddSingleton<ChatService>();
            collection.AddSingleton<ClientService>();
            collection.AddSingleton<GroupService>();
        }
    }
}
