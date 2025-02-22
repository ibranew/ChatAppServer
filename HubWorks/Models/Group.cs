namespace ChatServerExample.HubWorks.Models
{
    public class Group
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; } = "";
        public List<Client> Members { get; set; } = new();
    }
}
