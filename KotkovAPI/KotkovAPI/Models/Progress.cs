namespace KotkovAPI.Models
{
    public class Progress
    {
        public int TestId { get; set; }
        public int StudentId { get; set; }
        public int Mark { get; set; }

        public Test? Test { get; set; }
        public Student? Student { get; set; }
    }
}