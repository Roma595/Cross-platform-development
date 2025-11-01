namespace KotkovAPI.Models
{
    public class Course
    {
        public int Id { get;}
        public required int TeacherId { get; set; }
        public required string Name { get; set; }
        public required int TotalPlaces { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }

        public Teacher? Teacher { get; set; }
        public List<Test> Tests { get; } = [];
        public List<Attendence> Attendences { get; } = [];
        public List<Student> Students { get; } = [];
    }
}
