namespace KotkovAPI.Models
{
    public class Person
    {
        public required string Login { get; set; }
        public required string Password { get; set; }
        public required string Role { get; set; }
    }

    public static class Roles
    {
        public const string Admin = "Администратор";
        public const string Teacher = "Учитель";
        public const string Student = "Студент";
    }
}