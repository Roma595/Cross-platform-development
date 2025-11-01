using KotkovAPI.DTOs;
using KotkovAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace KotkovAPI.Data.Services
{
    public class TestService
    {
        private readonly KotkovAPIContext _context;

        public TestService(KotkovAPIContext context)
        {
            _context = context;
        }
        private static TestResponseDTO TestToResponseDTO(Test test) => new TestResponseDTO
        {
            Id = test.Id,
            CourseId = test.CourseId,
            Name = test.Name,
            HighestMark = test.HighestMark
        };
        private static TestByCourseDTO TestToTestByCourseDTO(Test test) => new TestByCourseDTO
        {
            Id = test.Id,
            Name = test.Name,
            HighestMark = test.HighestMark
        };
        public IEnumerable<TestResponseDTO> GetAll()
        {
            return _context.Tests.Select(t => TestToResponseDTO(t)).ToList();
        }
        public TestResponseDTO? GetById(int id)
        {
            var test = _context.Tests
                .FirstOrDefault(t => t.Id == id);
            if (test == null)
            {
                return null;
            }
            return TestToResponseDTO(test);
        }
        public TestResponseDTO? Create(CreateTestDTO testDTO)
        {
            var course = _context.Courses.Find(testDTO.CourseId);
            if (course == null)
            {
                return null;
            }
            var test = new Test
            {
                CourseId = testDTO.CourseId,
                Name = testDTO.Name,
                HighestMark = testDTO.HighestMark
            };

            _context.Tests.Add(test);
            _context.SaveChanges();

            return TestToResponseDTO(test);
        }
        public TestResponseDTO? Update(int id, UpdateTestDTO testDTO)
        {
            var course = _context.Courses.Find(testDTO.CourseId);
            if (course == null)
            {
                return null;
            }
            var existingTest = _context.Tests.Find(id);
            if (existingTest != null)
            {
                existingTest.CourseId = testDTO.CourseId;
                existingTest.Name = testDTO.Name;
                existingTest.HighestMark = testDTO.HighestMark;

                _context.SaveChanges();
                return TestToResponseDTO(existingTest);
            }
            return null;
        }
        public bool Delete(int id)
        {
            var test = _context.Tests.Find(id);
            if (test != null)
            {
                try
                {
                    _context.Tests.Remove(test);
                    _context.SaveChanges();
                }
                catch (DbUpdateException)
                {
                    return false;
                }
            }
            return true;
        }

        public IEnumerable<TestByCourseDTO> GetTestsByCourseId(int courseId)
        {
            var course = _context.Courses.Include(c => c.Tests).FirstOrDefault(c => c.Id == courseId);
            if (course == null)
            {
                return [];
            }

            var tests = course.Tests;

            if (tests == null || !tests.Any())
            {
                return [];
            }

            var testsDTOs = tests.Select(t => TestToTestByCourseDTO(t)).ToList();

            return testsDTOs;
        }

        public IEnumerable<TestProgressDTO> GetStudentsProgressForTest(int testId)
        {
            var test = _context.Tests.Include(t => t.Students).FirstOrDefault(t => t.Id == testId);
            if (test == null)
            {
                return [];
            }
            var progresses = test.Students.Select(s => new TestProgressDTO
            {
                FirstName = s.FirstName,
                LastName = s.LastName,
                Mark = s.Progresses.First(p => p.TestId == testId).Mark
            }).ToList();
            if (progresses == null || !progresses.Any())
            {
                return [];
            }
            return progresses;

        }

    }
}