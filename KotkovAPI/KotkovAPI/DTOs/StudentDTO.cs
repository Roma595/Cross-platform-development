namespace KotkovAPI.DTOs
{
    public class StudentDTO
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public string? PhoneNumber { get; set; }
    }

    // create student dto

    public class PushStudentToCourseDTO
    {
        public required int StudentId { get; set; }
        public required int CourseId { get; set; }
        public int StatusId { get; set; }
    }

    public class GetAllStudentsByCourseDTO
    {
        public int Id { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
    }
}