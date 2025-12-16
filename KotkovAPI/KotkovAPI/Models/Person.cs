namespace KotkovAPI.Models
{
    public class Person
    {
        public required string Login { get; set; }
        public required string Password { get; set; }
        public required string Role { get; set; }
        public int UserId { get; set; }
    }

    public static class Roles
    {
        public const string Admin = "Admin";
        public const string Teacher = "Teacher";
        public const string Student = "Student";
    }
}