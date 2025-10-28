using System.ComponentModel.DataAnnotations;

namespace KotkovAPI.Models
{
    public class Teacher
    {
        public int Id { get;}
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public string? PhoneNumber { get; set; }
        public List<Course> Courses { get; } = [];
    }
}