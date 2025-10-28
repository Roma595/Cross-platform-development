using System.ComponentModel.DataAnnotations;

namespace KotkovAPI.DTOs
{
    public class TestDTO
    {
        [Required]
        public int CourseId { get; set; }
        [Required]
        public required string Name { get; set; }
        [Required]
        public int HighestMark { get; set; }
    }
}