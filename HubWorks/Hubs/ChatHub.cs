using ChatServerExample.HubWorks.Models;
using ChatServerExample.HubWorks.HubInterfaces;
using Microsoft.AspNetCore.SignalR;
using System.Reflection;
using ChatServerExample.HubWorks.Services.Concretes;
using ChatServerExample.HubWorks.Consts;

namespace ChatServerExample.HubWorks.Hubs
{

    public class ChatHub : Hub<IChatHub>
    {
        ChatService _chatService;

        public ChatHub(ChatService chatService)
        {
            _chatService = chatService;
        }

        //Client
        public async Task AddClient(string name)
        {
            bool reseult = _chatService.AddClient(name, Context.ConnectionId);
            if (reseult)
            {
                await NotificationToCaller(name + " Hoş geldin", true);
                //login succes
                await Clients.Caller.ReceiveLoginNotificationAsync(name);
                await UpdateGroups();
                await UpdateClients();
            }
            else
            {
                await NotificationToCaller($"({name}) Bu isim daha önceden alınmıştır", false);

            }

        }
        public async Task LeaveClient()
        {
            _chatService.LeavedClient(Context.ConnectionId);
            await UpdateClients();
            await UpdateGroups();
        }

        //group
        public async Task AddGroup(string groupName)
        {
            Group? group = _chatService.AddGroup(groupName, Context.ConnectionId);
            //grup eklenmişse
            if (group is not null)
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
                await UpdateGroups();
            }
            else
            {
                await NotificationToCaller($"({groupName}) Bu grup zaten mevcut", false);
            }


        }
        //hub metot name change
        [HubMethodName("JoinGroup")]
        public async Task JoinToGroup(string groupId)
        {
            Group? group = _chatService.JoinClientToGroup(groupId, Context.ConnectionId);
            if (group is not null)
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, group.Name);
                await NotificationToCaller($"{group.Name} adlı gruba Katıldın", true);
                await UpdateGroups();
            }
            else
                await NotificationToCaller($"Gruba Katılma işlem gerçekleştirelemedi", false);

        }

        //send message
        public async Task SendMessage(string message, string clientName)
        {
            var targetId = _chatService.GetClientConnectionIdByClientName(clientName);
            string? sender = _chatService.GetClientNameByConnectionId(Context.ConnectionId);
            if (targetId is null || sender is null)
                return;
                await Clients.Client(targetId).ReceiveMessageAsync(message, sender);
            
        }
        public async Task SendMessageToGroup(string message, string groupId)
        {
            var groupName = _chatService.GetGroupNameByGroupId(groupId);
            string? sender = _chatService.GetClientNameByConnectionId(Context.ConnectionId);
            if (groupName is null || sender is null)
                return;

            await Clients.OthersInGroup(groupName).ReceiveMessageGroupAsync(message, sender, groupId);
        }

        //Notifications
        private async Task NotificationToCaller(string message, bool success)
        => await Clients.Caller.ReceiveNotificationAsync(message, success);
        private async Task NotificationToOthers(string message, bool success)
           => await Clients.Others.ReceiveNotificationAsync(message, success);

        //Update
        private async Task UpdateClients()
        {
            List<string> clients = _chatService.GetAllClients();
            await Clients.All.ReceiveClientsAsync(clients);
        }
        private async Task UpdateGroups()
        {
            List<DTOs.GroupDto> groups = _chatService.GetAllGroups(Context.ConnectionId);
            await Clients.All.ReceiveGroupsAsync(groups);
        }
        //connection

        public override async Task OnConnectedAsync()
        {
            // Tüm Client'lar "Tümü" adlı gruba üyedir
            await Groups.AddToGroupAsync(Context.ConnectionId,GenaralGroupNames.GeneralGroupName);
            await UpdateClients();
            await UpdateGroups();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            await LeaveClient();
        }

        
    }
}
