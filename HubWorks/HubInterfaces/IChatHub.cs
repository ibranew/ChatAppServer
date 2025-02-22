namespace ChatServerExample.HubWorks.HubInterfaces
{
    public interface IChatHub
    {
        Task ReceiveLoginNotificationAsync(string name);
        Task ReceiveMessageAsync(string message, string sender);
        Task ReceiveMessageGroupAsync(string message, string sender,string groupName);
        Task ReceiveNotificationAsync(string message, bool success);
        Task ReceiveClientsAsync(List<string> clients);
        Task ReceiveGroupsAsync(List<DTOs.GroupDto> groups);
    }
}
