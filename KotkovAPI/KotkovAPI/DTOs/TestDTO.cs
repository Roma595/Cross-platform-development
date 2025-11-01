using System.ComponentModel.DataAnnotations;

namespace KotkovAPI.DTOs
{
    public class TestResponseDTO
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        public string? Name { get; set; }
        public int HighestMark { get; set; }
    }

    public class CreateTestDTO
    {
        public required int CourseId { get; set; }
        [MaxLength(45)]
        public required string Name { get; set; }
        public required int HighestMark { get; set; }
    }
    public class UpdateTestDTO
    {
        public required int CourseId { get; set; }
        [MaxLength(45)]
        public required string Name { get; set; }
        public required int HighestMark { get; set; }
    }

    public class TestProgressDTO
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required int Mark { get; set; }
    }

    public class TestByCourseDTO
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required int HighestMark { get; set; }
    }

}