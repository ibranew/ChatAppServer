using ChatServerExample.HubWorks.Models;
using ChatServerExample.HubWorks.DTOs;
namespace ChatServerExample.HubWorks.Services.Concretes
{
    public class ClientService
    {
        private List<Client> Clients { get; set; } = new();
        public ClientService() { }

        public Client? AddClient(string name, string connectionId)
        {
            //daha önce tanımlanmış isimler eklenmez
            if (Clients.Any(c => c.Name == "Tümü" || c.Name == name))
            {
                return null;
            }

            Client client = new() { Name = name, ConnectionId = connectionId };
            Clients.Add(client);
            return client;
        }
        public void LeaveClient(Client client)
        {
            Clients.Remove(client);
        }
        public Client? GetClientByConnectionId(string connectionId)
        {
            return Clients.FirstOrDefault(c => c.ConnectionId == connectionId);
        }

        public string? GetClientConnectionIdByName(string name)
        {
            Client? client = Clients.FirstOrDefault(c => c.Name == name);
            if (client is not null)
                return client.ConnectionId;

            return null;
        }
        public void RemoveClientByConnectionId(string ConnectionId)
        {
            Client? client = Clients.FirstOrDefault(c => c.ConnectionId == ConnectionId);
            if (client is not null)
                Clients.Remove(client);
        }
        public List<string> GetAllClientNames()
        {
            return Clients
                .Select(c => c.Name)  // Directly select the Name property
                .ToList();
        }

    }
}
