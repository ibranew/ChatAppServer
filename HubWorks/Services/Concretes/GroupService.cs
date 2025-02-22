using ChatServerExample.HubWorks.Consts;
using ChatServerExample.HubWorks.DTOs;
using ChatServerExample.HubWorks.Models;
using System.Collections.Concurrent;
using System.Xml.Linq;

namespace ChatServerExample.HubWorks.Services.Concretes
{
    public class GroupService
    {
        public List<Group> _Groups { get; set; } = new();
        public Group AllClientsGroup { get; set; }
        public GroupService()
        {
            AllClientsGroup = new Group() { Name = GenaralGroupNames.GeneralGroupName};
            _Groups.Add(AllClientsGroup);
        }
        public void AddGeneralGroup(Client client){
          AllClientsGroup.Members.Add(client);
        }
        public Group? AddGroup(string groupName)
        {
            if (_Groups.Any(c => c.Name == groupName))
            {
                return null;
            }
            Group group = new() { Name = groupName };
            _Groups.Add(group);
            return group;
        }
        public Group? AddClientToGroup(string groupId, Client client)
        {
            Group? group = _Groups.FirstOrDefault(g => g.Id == groupId);
            if (group is not null)
            {
                group.Members.Add(client);
                return group;
            }
            else
                return null;
        }
        public void RemoveClientFromAllGroups(Client client)
        {
            foreach (var group in _Groups)
            {
                // Eğer client Members koleksiyonunda varsa sil
                if (group.Members.Contains(client))
                {
                    group.Members.Remove(client);
                }
            }
        }
        public List<GroupDto> GetAllGroupsWithOrderByClientMemberShip(string connectionId)
        {
            return _Groups
                .OrderByDescending(g => g.Name == "Tümü") // "Tümü" grubunu en uste koyar
                .ThenByDescending(g => g.Members.Any(m => m.ConnectionId == connectionId)) // Client'in üye olduğu gruplari yukari al
                .Select(g => new GroupDto
                {
                    Id = g.Id,
                    Name = g.Name,
                    MemberNames = g.Members.Select(m => m.Name).ToList()
                })
                .ToList();
        }
        public string? GetGroupNameById(string id)
        {
            return _Groups.FirstOrDefault(g => g.Id == id)?.Name;
        }

    }
}
