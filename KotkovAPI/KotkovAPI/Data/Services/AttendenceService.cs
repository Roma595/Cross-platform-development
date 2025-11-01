using KotkovAPI.DTOs;
using KotkovAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace KotkovAPI.Data.Services
{
    public class AttendenceService
    {
        private readonly KotkovAPIContext _context;

        public AttendenceService(KotkovAPIContext context)
        {
            _context = context;
        }

        private static AttendenceResponseDTO AttendenceToDTO(Attendence attendence) =>
            new AttendenceResponseDTO
            {
                StudentId = attendence.StudentId,
                CourseId = attendence.CourseId,
                StatusId = attendence.StatusId
            };
        public AttendenceResponseDTO? GetByIds(int studentId, int courseId)
        {
            var attendence = _context.Attendences
                .FirstOrDefault(s => s.StudentId == studentId && s.CourseId == courseId);
            if (attendence == null)
            {
                return null;
            }
            return AttendenceToDTO(attendence);
        }
        public AttendenceResponseDTO? Create(int studentId, int courseId, int statusId)
        {
            if (!_context.Students.Any(s => s.Id == studentId) || !_context.Courses.Any(c => c.Id == courseId))
            {
                return null;
            }
            if (_context.Attendences.Any(s => s.StudentId == studentId && s.CourseId == courseId))
            {
                return null;
            }
            var attendence = new Attendence
            {
                StudentId = studentId,
                CourseId = courseId,
                StatusId = statusId
            };
            _context.Attendences.Add(attendence);
            _context.SaveChanges();
            return AttendenceToDTO(attendence);
        }
        public bool Delete(int studentId, int courseId)
        {
            var attendence = _context.Attendences.First(s => s.StudentId == studentId && s.CourseId == courseId);
            if (attendence != null)
            {
                try
                {
                    _context.Attendences.Remove(attendence);
                    _context.SaveChanges();
                }
                catch (DbUpdateException)
                {
                    return false;
                }
            }
            return true;
        }
    }
}