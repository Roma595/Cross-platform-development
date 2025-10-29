using System.ComponentModel.DataAnnotations;

namespace KotkovAPI.DTOs
{
    public class TeacherResponseDTO
    {
        public int Id { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public string? PhoneNumber { get; set; }
    }

    public class CreateTeacherDTO
    {
        [MaxLength(45)]
        public required string FirstName { get; set; }
        [MaxLength(45)]
        public required string LastName { get; set; }
        [MaxLength(20)]
        public string? PhoneNumber { get; set; }
    }
    public class UpdateTeacherDTO
    {
        [MaxLength(45)]
        public required string FirstName { get; set; }
        [MaxLength(45)]
        public required string LastName { get; set; }
        [MaxLength(20)]
        public string? PhoneNumber { get; set; }
    }
}