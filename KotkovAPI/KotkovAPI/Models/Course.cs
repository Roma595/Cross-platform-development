namespace KotkovAPI.Models
{
    public class Course
    {
        public int Id { get;}
        public required int TeacherId { get; set; }
        public required string Name { get; set; }
        public required int TotalPlaces { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public Teacher? Teacher { get; set; }
        public List<Test> Tests { get; } = [];
        public List<Attendence> Attendences { get; } = [];
        public List<Student> Students { get; } = [];
    }
}
