namespace KotkovAPI.Data.Services
{
    using KotkovAPI.Models;
    using Microsoft.AspNetCore.Identity;
    using KotkovAPI.DTOs;
    using Microsoft.EntityFrameworkCore;

    public class StudentService
    {
        private readonly KotkovAPIContext _context;
        private readonly AttendenceService _attendenceService;
        public StudentService(KotkovAPIContext context)
        {
            _attendenceService = new AttendenceService(context);
            _context = context;
        }

        private static StudentDTO StudentToDTO(Student student) => new StudentDTO
        {
            FirstName = student.FirstName,
            LastName = student.LastName,
            PhoneNumber = student.PhoneNumber
        };

        private static GetAllStudentsByCourseDTO StudentToGetAllStudentsByCourseDTO(Student student) => new GetAllStudentsByCourseDTO
        {
            Id = student.Id,
            FirstName = student.FirstName,
            LastName = student.LastName
        };
        public IEnumerable<StudentDTO> GetAll()
        {
            return _context.Students.Select(s => StudentToDTO(s)).ToList();
        }
        public StudentDTO? GetById(int id)
        {
            var student = _context.Students
                .FirstOrDefault(s => s.Id == id);
            if (student == null)
            {
                return null;
            }
            return StudentToDTO(student);
        }

        public Student Create(StudentDTO studentDTO)
        {
            var student = new Student
            {
                FirstName = studentDTO.FirstName,
                LastName = studentDTO.LastName,
                PhoneNumber = studentDTO.PhoneNumber
            };

            _context.Students.Add(student);
            _context.SaveChanges();

            return student;
        }
        public StudentDTO? Update(int id, StudentDTO studentDTO)
        {
            var existingStudent = _context.Students.Find(id);
            if (existingStudent != null)
            {
                existingStudent.FirstName = studentDTO.FirstName;
                existingStudent.LastName = studentDTO.LastName;
                existingStudent.PhoneNumber = studentDTO.PhoneNumber;

                _context.SaveChanges();
                return StudentToDTO(existingStudent);
            }
            return null;
        }
        public void Delete(int id)
        {
            var student = _context.Students.Find(id);
            if (student != null)
            {
                _context.Students.Remove(student);
                _context.SaveChanges();
            }
        }

        

        public AttendenceResponseDTO? PushStudentToCourse(int studentId, int courseId, int statusId)
        {
            var student = _context.Students.Find(studentId);
            var course = _context.Courses.Include(c => c.Students).First(c => c.Id == courseId);

            if (student == null || course == null)
            {
                return null;
            }
            if (!course.Students.Any(s => s.Id == studentId))
            {
                var attendence = _attendenceService.Create(studentId, courseId, statusId);
                _context.SaveChanges();
                return attendence;
            }
            return null;
        }

        public IEnumerable<GetAllStudentsByCourseDTO> GetAllStudentsByCourse(int courseId)
        {
            var course = _context.Courses.Find(courseId);
            if (course == null)
            {
                return Enumerable.Empty<GetAllStudentsByCourseDTO>();
            }

            var students = _context.Students.Where(s => s.Courses.Any(c => c.Id == courseId)).ToList();
            if (students == null || !students.Any())
            {
                return Enumerable.Empty<GetAllStudentsByCourseDTO>();
            }
            var studentsDTOs = students.Select(s => StudentToGetAllStudentsByCourseDTO(s)).ToList();
            return studentsDTOs;
        }
    }
}