using ChatServerExample.HubWorks.Hubs;
using ChatServerExample.HubWorks.Models;
using Microsoft.AspNetCore.SignalR;
using ChatServerExample.HubWorks.DTOs;


namespace ChatServerExample.HubWorks.Services.Concretes
{
    public class ChatService
    {
        GroupService _groupService;
        ClientService _clientService;

        public ChatService(
            GroupService groupService,
            ClientService clientService
           )
        {
            _clientService = clientService;
            _groupService = groupService;

        }

        public bool AddClient(string clientName, string connectionId)
        {
            Client? client = _clientService.AddClient(clientName, connectionId);
            if(client is not null)
            {
              _groupService.AddGeneralGroup(client);
               return true;
            }
            return false;
        }
        public void LeavedClient(string connectionId)
        {
            Client? client = _clientService.GetClientByConnectionId(connectionId);
            if (client is not null)
            {
                //Client tüm gruplardan çıkarılıyor
                _groupService.RemoveClientFromAllGroups(client);
                //Client clients'lardan çıkarılıyor
                _clientService.LeaveClient(client);
            }

        }
        public List<string> GetAllClients()
        {
            return _clientService.GetAllClientNames();
        }
        public List<GroupDto> GetAllGroups(string connectionId)
        {
            return _groupService.GetAllGroupsWithOrderByClientMemberShip(connectionId);
        }
        public Group? JoinClientToGroup(string groupId, string connectionId)
        {

            Client? client = _clientService.GetClientByConnectionId(connectionId);

            if (client is not null)
            {
                return _groupService.AddClientToGroup(groupId, client); ;
            }
            return null;
        }
        public Group? AddGroup(string groupName, string connectionId)
        {
            //grup yoksa ekler varsa eklemez
            Group? group = _groupService.AddGroup(groupName);

            Client? client = _clientService.GetClientByConnectionId(connectionId);

            if (client is not null && group is not null)
            {
                _groupService.AddClientToGroup(group.Id, client);
                return group;
            }
            return null;
        }
        public string? GetClientNameByConnectionId(string connectionId)
        {
            return _clientService.GetClientByConnectionId(connectionId)?.Name;
        }
        public (string? conversationName, bool IsGroupName) GetConnectionIdOrGroupNameByIdOrClientName(string idOrName)
        {
            string? connectionId = GetClientConnectionId(idOrName);
            if (connectionId is not null)
            {
                return (connectionId, false);
            }

            string? groupName = GetGroupName(idOrName);
            if (groupName is not null)
            {
                return (groupName, true);
            }

            return (null, false);
        }
       
        public string? GetGroupNameByGroupId(string id)
        {
            return _groupService.GetGroupNameById(id);
        }
        public string? GetClientConnectionIdByClientName(string name)
        {
            return _clientService.GetClientConnectionIdByName(name);
        }

        private bool IsGuid(string value)
        {
            return Guid.TryParse(value, out _);
        }
        private string? GetClientConnectionId(string idOrName)
        {
            if (IsGuid(idOrName)) return null; // Eğer bu bir GUID ise, direkt kullanıcı ismi değildir.
            return _clientService.GetClientConnectionIdByName(idOrName);
        }
        private string? GetGroupName(string idOrName)
        {
            if (!IsGuid(idOrName)) return null; // Eğer bu bir GUID değilse, grup ID'si de olamaz.
            return _groupService.GetGroupNameById(idOrName);
        }

    }
}
