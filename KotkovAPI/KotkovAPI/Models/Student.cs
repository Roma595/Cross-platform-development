namespace KotkovAPI.Models
{
    public class Student
    {
        public int Id { get;}
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public string? PhoneNumber { get; set; }
        public List<Attendence> Attendences { get; } = [];
        public List<Course> Courses { get; } = [];
        public List<Progress> Progresses { get; } = [];
        public List<Test> Tests { get; } = [];
    }
}