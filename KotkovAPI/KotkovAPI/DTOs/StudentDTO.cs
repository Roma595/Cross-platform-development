using System.ComponentModel.DataAnnotations;

namespace KotkovAPI.DTOs
{
    public class StudentResponseDTO
    {
        public int Id { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public string? PhoneNumber { get; set; }
    }
    public class CreateStudentDTO
    {
        [MaxLength(50)]
        public required string FirstName { get; set; }
        [MaxLength(50)]
        public required string LastName { get; set; }
        [MaxLength(20)]
        public string? PhoneNumber { get; set; }
    }
    public class UpdateStudentDTO
    {
        [MaxLength(50)]
        public required string FirstName { get; set; }
        [MaxLength(50)]
        public required string LastName { get; set; }
        [MaxLength(20)]
        public string? PhoneNumber { get; set; }
    }
    public class AddStudentToCourseDTO
    {
        public required int StudentId { get; set; }
        public required int CourseId { get; set; }
        public required int StatusId { get; set; }
    }
    public class StudentByCourseDTO
    {
        public int Id { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
    }
}