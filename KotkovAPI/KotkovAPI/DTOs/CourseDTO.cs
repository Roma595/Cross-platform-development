using System.ComponentModel.DataAnnotations;

namespace KotkovAPI.DTOs
{
    public class CourseResponseDTO
    {
        public int Id { get; set; }
        public int TeacherId { get; set; }
        public required string Name { get; set; }
        public int TotalPlaces { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
    public class CreateCourseDTO
    {
        public required int TeacherId { get; set; }
        [MaxLength(45)]
        public required string Name { get; set; }
        public required int TotalPlaces { get; set; }
        public required DateTime StartDate { get; set; }
        public required DateTime EndDate { get; set; }
    }

    public class UpdateCourseDTO
    {
        public required int TeacherId { get; set; }
        [MaxLength(45)]
        public required string Name { get; set; }
        public required int TotalPlaces { get; set; }
        public required DateTime StartDate { get; set; }
        public required DateTime EndDate { get; set; }
    }

    public class CourseByStudentDTO
    {
        public int Id { get; set; }
        public int TeacherId { get; set; }
        public required string Name { get; set; }
        public int TotalPlaces { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
