namespace ChatServerExample.HubWorks.DTOs
{
    public class GroupDto
    {
        public string? Id { get; set; } 
        public string? Name { get; set; }
        public List<string> MemberNames { get; set; } = new();
    }

}
