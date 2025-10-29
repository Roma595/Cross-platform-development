namespace KotkovAPI.Data.Services
{
    using KotkovAPI.Models;
    using KotkovAPI.DTOs;
    using Microsoft.EntityFrameworkCore;
    public class TeacherService
    {
        private readonly KotkovAPIContext _context;
        public TeacherService(KotkovAPIContext context)
        {
            _context = context;
        }

        private static TeacherResponseDTO TeacherToResponseDTO(Teacher teacher) =>
            new TeacherResponseDTO
            {
                Id = teacher.Id,
                FirstName = teacher.FirstName,
                LastName = teacher.LastName,
                PhoneNumber = teacher.PhoneNumber
            };

        public IEnumerable<TeacherResponseDTO> GetAll()
        {
            return _context.Teachers.Select(t => TeacherToResponseDTO(t)).ToList();
        }
        public TeacherResponseDTO? GetById(int id)
        {
            var teacher = _context.Teachers
                .FirstOrDefault(t => t.Id == id);
            if (teacher == null)
            {
                return null;
            }
            return TeacherToResponseDTO(teacher);
        }
        public Teacher Create(CreateTeacherDTO teacherDTO)
        {
            var teacher = new Teacher
            {
                FirstName = teacherDTO.FirstName,
                LastName = teacherDTO.LastName,
                PhoneNumber = teacherDTO.PhoneNumber
            };

            _context.Teachers.Add(teacher);
            _context.SaveChanges();

            return teacher;
        }
        public TeacherResponseDTO? Update(int id, UpdateTeacherDTO teacherDTO)
        {
            var existingTeacher = _context.Teachers.Find(id);
            if (existingTeacher != null)
            {
                existingTeacher.FirstName = teacherDTO.FirstName;
                existingTeacher.LastName = teacherDTO.LastName;
                existingTeacher.PhoneNumber = teacherDTO.PhoneNumber;
                _context.SaveChanges();

                return TeacherToResponseDTO(existingTeacher);
            }
            return null;
        }
        public void Delete(int id)
        {
            var teacher = _context.Teachers.Find(id);
            if (teacher != null)
            {
                _context.Teachers.Remove(teacher);
                _context.SaveChanges();
            }
        }
    }
}