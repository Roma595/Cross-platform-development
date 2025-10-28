using System.ComponentModel.DataAnnotations;

namespace KotkovAPI.DTOs
{
     public class TeacherDTO
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        [MaxLength(20)]
        public string? PhoneNumber { get; set; }
    }
}