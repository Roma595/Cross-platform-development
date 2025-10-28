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

        private static TeacherDTO TeacherToDTO(Teacher teacher) =>
            new TeacherDTO
            {
                FirstName = teacher.FirstName,
                LastName = teacher.LastName,
                PhoneNumber = teacher.PhoneNumber
            };

        public IEnumerable<TeacherDTO> GetAll()
        {
            return _context.Teachers.Select(t => TeacherToDTO(t)).ToList();
        }

        public TeacherDTO? GetById(int id)
        {
            var teacher = _context.Teachers
                .FirstOrDefault(t => t.Id == id);
            if (teacher == null)
            {
                return null;
            }
            return TeacherToDTO(teacher);
        }

        public Teacher Create(TeacherDTO teacherDTO)
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

        public TeacherDTO? Update(int id, TeacherDTO teacherDTO)
        {
            var existingTeacher = _context.Teachers.Find(id);
            if (existingTeacher != null)
            {
                existingTeacher.FirstName = teacherDTO.FirstName;
                existingTeacher.LastName = teacherDTO.LastName;
                existingTeacher.PhoneNumber = teacherDTO.PhoneNumber;
                _context.SaveChanges();

                return teacherDTO;
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