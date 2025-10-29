using KotkovAPI.DTOs;
using KotkovAPI.Models;

namespace KotkovAPI.Data.Services
{
    public class TestService
    {
        private readonly KotkovAPIContext _context;

        public TestService(KotkovAPIContext context)
        {
            _context = context;
        }
        private static TestResponseDTO TestToResponseDTO(Test test) => new TestResponseDTO {
            Id = test.Id,
            CourseId = test.CourseId,
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
        public Test? Create(CreateTestDTO testDTO)
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

            return test;
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
        public void Delete(int id)
        {
            var test = _context.Tests.Find(id);
            if (test != null)
            {
                _context.Tests.Remove(test);
                _context.SaveChanges();
            }
        }
    }
}