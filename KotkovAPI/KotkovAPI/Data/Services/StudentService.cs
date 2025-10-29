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

        private static StudentResponseDTO StudentToResponseDTO(Student student) => new StudentResponseDTO
        {
            Id = student.Id,
            FirstName = student.FirstName,
            LastName = student.LastName,
            PhoneNumber = student.PhoneNumber
        };
        private static StudentByCourseDTO StudentToStudentByCourseDTO(Student student) => new StudentByCourseDTO
        {
            Id = student.Id,
            FirstName = student.FirstName,
            LastName = student.LastName
        };

        public IEnumerable<StudentResponseDTO> GetAll()
        {
            return _context.Students.Select(s => StudentToResponseDTO(s)).ToList();
        }
        public StudentResponseDTO? GetById(int id)
        {
            var student = _context.Students
                .FirstOrDefault(s => s.Id == id);
            if (student == null)
            {
                return null;
            }
            return StudentToResponseDTO(student);
        }
        public Student Create(CreateStudentDTO studentDTO)
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
        public StudentResponseDTO? Update(int id, UpdateStudentDTO studentDTO)
        {
            var existingStudent = _context.Students.Find(id);
            if (existingStudent != null)
            {
                existingStudent.FirstName = studentDTO.FirstName;
                existingStudent.LastName = studentDTO.LastName;
                existingStudent.PhoneNumber = studentDTO.PhoneNumber;

                _context.SaveChanges();
                return StudentToResponseDTO(existingStudent);
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

        public AttendenceResponseDTO? AddStudentToCourse(AddStudentToCourseDTO attendenceDTO)
        {
            var student = _context.Students.Find(attendenceDTO.StudentId);
            var course = _context.Courses.Include(c => c.Students).First(c => c.Id == attendenceDTO.CourseId);

            if (student == null || course == null)
            {
                return null;
            }
            if (!course.Students.Any(s => s.Id == attendenceDTO.StudentId))
            {
                var attendence = _attendenceService.Create(attendenceDTO.StudentId, attendenceDTO.CourseId, attendenceDTO.StatusId);
                _context.SaveChanges();
                return attendence;
            }
            return null;
        }
        public IEnumerable<StudentByCourseDTO> GetAllStudentsByCourse(int courseId)
        {
            var course = _context.Courses.Find(courseId);
            if (course == null)
            {
                return [];
            }

            var students = _context.Students.Where(s => s.Courses.Any(c => c.Id == courseId)).ToList();
            if (students == null || !students.Any())
            {
                return [];
            }
            var studentsDTOs = students.Select(s => StudentToStudentByCourseDTO(s)).ToList();
            return studentsDTOs;
        }
    }
}