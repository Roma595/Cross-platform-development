namespace KotkovAPI.Models
{
    public class Status
    {
        public int Id { get;}
        public required string Name { get; set; }
        public List<Attendence> Attendences { get; } = [];
    }
}