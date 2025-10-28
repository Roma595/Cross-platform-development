namespace KotkovAPI.Models
{
    public class Attendence
    {
        public required int StudentId { get; set; }
        public required int CourseId { get; set; }
        public required int StatusId { get; set; }

        public Student? Student { get; set; }
        public Course? Course { get; set; }
        public Status? Status { get; set; }
    }
}