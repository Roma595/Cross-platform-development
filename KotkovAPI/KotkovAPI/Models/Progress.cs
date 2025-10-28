namespace KotkovAPI.Models
{
    public class Progress
    {
        public int TestId { get; set; }
        public int StudentId { get; set; }
        public int Mark { get; set; }

        public required Test Test { get; set; }
        public required Student Student { get; set; }
    }
}