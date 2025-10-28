using System.ComponentModel.DataAnnotations;

namespace KotkovAPI.DTOs
{
    public class CourseDTO
    {
        public int TeacherId { get; set; }
        public required string Name { get; set; }
        public int TotalPlaces { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }

    public class CourseResponseDTO
    {
        public int Id { get; set; }
        public int TeacherId { get; set; }
        public required string Name { get; set; }
        public int TotalPlaces { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
