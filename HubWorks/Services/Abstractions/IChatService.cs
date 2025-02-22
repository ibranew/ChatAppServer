using ChatServerExample.HubWorks.DTOs;
using ChatServerExample.HubWorks.Models;

namespace ChatServerExample.HubWorks.Services.Abstractions
{
    public interface IChatService
    {
        bool AddClient(string clientName, string connectionId);
        void LeavedClient(string connectionId);
        List<string> GetAllClients();
        List<GroupDto> GetAllGroups(string connectionId);
        Group? JoinClientToGroup(string groupId, string connectionId);
        Group? AddGroup(string groupName, string connectionId);
        string? GetClientNameByConnectionId(string connectionId);
        (string? conversationName, bool IsGroupName) GetConnectionIdOrGroupNameByIdOrClientName(string idOrName);
        string? GetClientConnectionId(string idOrName);
    }
}
