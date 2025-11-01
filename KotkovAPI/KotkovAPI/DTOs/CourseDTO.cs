using System.ComponentModel.DataAnnotations;

namespace KotkovAPI.DTOs
{
    public class CourseResponseDTO
    {
        public int Id { get; set; }
        public int TeacherId { get; set; }
        public required string Name { get; set; }
        public int TotalPlaces { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
    }
    public class CreateCourseDTO
    {
        public required int TeacherId { get; set; }
        [MaxLength(45)]
        public required string Name { get; set; }
        public required int TotalPlaces { get; set; }
        public required DateOnly StartDate { get; set; }
        public required DateOnly EndDate { get; set; }
    }

    public class UpdateCourseDTO
    {
        public required int TeacherId { get; set; }
        [MaxLength(45)]
        public required string Name { get; set; }
        public required int TotalPlaces { get; set; }
        public required DateOnly StartDate { get; set; }
        public required DateOnly EndDate { get; set; }
    }

    public class CourseByStudentDTO
    {
        public required string Name { get; set; }
    }
}
