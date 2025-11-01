using KotkovAPI.DTOs;
using KotkovAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace KotkovAPI.Data.Services
{
    public class ProgressService
    {
        private readonly KotkovAPIContext _context;

        public ProgressService(KotkovAPIContext context)
        {
            _context = context;
        }

        private static ProgressResponseDTO ProgressToDTO(Progress progress) =>
            new ProgressResponseDTO
            {
                TestId = progress.TestId,
                StudentId = progress.StudentId,
                Mark = progress.Mark
            };
        public ProgressResponseDTO? GetByIds(int testId, int studentId)
        {
            var progress = _context.Progresses
                .FirstOrDefault(p => p.TestId == testId && p.StudentId == studentId);
            if (progress == null)
            {
                return null;
            }
            return ProgressToDTO(progress);
        }
        public ProgressResponseDTO? Create(int testId, int studentId, int mark)
        {
            if (!_context.Tests.Any(t => t.Id == testId) || !_context.Students.Any(s => s.Id == studentId))
            {
                return null;
            }
            if (_context.Progresses.Any(p => p.TestId == testId && p.StudentId == studentId))
            {
                return null;
            }
            var progress = new Progress
            {
                TestId = testId,
                StudentId = studentId,
                Mark = mark
            };
            _context.Progresses.Add(progress);
            _context.SaveChanges();
            return ProgressToDTO(progress);
        }
        public bool Delete(int testId, int studentId)
        {
            var progress = _context.Progresses.First(p => p.TestId == testId && p.StudentId == studentId);
            if (progress != null)
            {
                try
                {
                    _context.Progresses.Remove(progress);
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