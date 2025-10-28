namespace KotkovAPI.Models
{
    public class Test
    {
        public int Id { get; }
        public int CourseId { get; set; }
        public required string Name { get; set; }
        public int HighestMark { get; set; }

        public Course? Course { get; set; }
        public List<Progress> Progresses { get; } = [];
        public List<Student> Students { get; } = [];
    }
}