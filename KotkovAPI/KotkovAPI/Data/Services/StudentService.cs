namespace KotkovAPI.Data.Services
{
    using KotkovAPI.Models;
    using KotkovAPI.DTOs;
    using Microsoft.EntityFrameworkCore;

    public class StudentService
    {
        private readonly KotkovAPIContext _context;
        private readonly AttendenceService _attendenceService;
        private readonly ProgressService _progressService;
        public StudentService(KotkovAPIContext context)
        {
            _attendenceService = new AttendenceService(context);
            _progressService = new ProgressService(context);
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
        public StudentResponseDTO Create(CreateStudentDTO studentDTO)
        {
            var student = new Student
            {
                FirstName = studentDTO.FirstName,
                LastName = studentDTO.LastName,
                PhoneNumber = studentDTO.PhoneNumber
            };

            _context.Students.Add(student);
            _context.SaveChanges();

            return StudentToResponseDTO(student);
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
        public bool Delete(int id)
        {
            var student = _context.Students.Find(id);
            if (student != null)
            {
                try
                {
                    _context.Students.Remove(student);
                    _context.SaveChanges();
                }
                catch (DbUpdateException)
                {
                    return false;
                }
            }
            return true;
        }

        public AttendenceResponseDTO? AddStudentToCourse(AddStudentToCourseDTO attendenceDTO)
        {
            var student = _context.Students.Find(attendenceDTO.StudentId);
            var course = _context.Courses.Include(c => c.Students).FirstOrDefault(c => c.Id == attendenceDTO.CourseId);

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
            var course = _context.Courses.Include(c => c.Students).FirstOrDefault(c => c.Id == courseId);
            if (course == null)
            {
                return [];
            }

            var students = course.Students;
            if (students == null || !students.Any())
            {
                return [];
            }
            var studentsDTOs = students.Select(s => StudentToStudentByCourseDTO(s)).ToList();
            return studentsDTOs;
        }

        public ProgressResponseDTO? AddStudentProgressForTest(AddStudentProgressForTestDTO progressDTO)
        {
            var student = _context.Students.Find(progressDTO.StudentId);
            var test = _context.Tests.Include(t => t.Students).FirstOrDefault(t => t.Id == progressDTO.TestId);

            if (student == null || test == null)
            {
                return null;
            }

            if (!test.Students.Any(s => s.Id == progressDTO.StudentId))
            {
                var progress = _progressService.Create(progressDTO.TestId, progressDTO.StudentId, progressDTO.Mark);
                _context.SaveChanges();
                return progress;
            }
            return null;
        }
        public IEnumerable<StudentProgressDTO> GetAllProgressesForStudent(int studentId)
        {
            var student = _context.Students.Include(s => s.Progresses).Include(s => s.Tests).ThenInclude(t => t.Course).FirstOrDefault(s => s.Id == studentId);
            if (student == null)
            {
                return [];
            }

            var progresses = student.Progresses.Select(p => new StudentProgressDTO
            {
                CourseName = p.Test!.Course!.Name,
                TestName = p.Test!.Name,
                Mark = p.Mark
            }).ToList();
            if (progresses == null || !progresses.Any())
            {
                return [];
            }
            return progresses;
        }
    }
}