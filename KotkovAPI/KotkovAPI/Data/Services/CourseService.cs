namespace KotkovAPI.Data.Services
{
    using KotkovAPI.Models;
    using KotkovAPI.DTOs;
    using Microsoft.EntityFrameworkCore;
    using System;

    public class CourseService
    {
        private readonly KotkovAPIContext _context;
        public CourseService(KotkovAPIContext context)
        {
            _context = context;
        }

        private static CourseResponseDTO CourseToResponseDTO(Course course) => new CourseResponseDTO
        {
            Id = course.Id,
            TeacherId = course.TeacherId,
            Name = course.Name,
            TotalPlaces = course.TotalPlaces,
            StartDate = course.StartDate,
            EndDate = course.EndDate
        };

        public IEnumerable<CourseResponseDTO> GetAll()
        {
            return _context.Courses.Select(c => CourseToResponseDTO(c)).ToList();
        }

        public CourseResponseDTO? GetById(int id)
        {
            var course = _context.Courses
                .FirstOrDefault(c => c.Id == id);
            if (course == null)
            {
                return null;
            }
            return CourseToResponseDTO(course);
        }

        public Course? Create(CreateCourseDTO courseDTO)
        {   
            var teacher = _context.Teachers.Find(courseDTO.TeacherId);
            if (teacher == null)
            {
                return null;
            }
            var course = new Course
            {
                TeacherId = courseDTO.TeacherId,
                Name = courseDTO.Name,
                TotalPlaces = courseDTO.TotalPlaces,
                StartDate = courseDTO.StartDate,
                EndDate = courseDTO.EndDate
            };

            _context.Courses.Add(course);
            _context.SaveChanges();

            return course;
        }

        public CourseResponseDTO? Update(int id, UpdateCourseDTO courseDTO)
        {
            var teacher = _context.Teachers.Find(courseDTO.TeacherId);
            if (teacher == null)
            {
                return null;
            }
            var existingCourse = _context.Courses.Find(id);
            if (existingCourse != null)
            {
                existingCourse.TeacherId = courseDTO.TeacherId;
                existingCourse.Name = courseDTO.Name;
                existingCourse.TotalPlaces = courseDTO.TotalPlaces;
                existingCourse.StartDate = courseDTO.StartDate;
                existingCourse.EndDate = courseDTO.EndDate;

                _context.SaveChanges();
                return CourseToResponseDTO(existingCourse);
            }
            return null;
        }

        public void Delete(int id)
        {
            var course = _context.Courses.Find(id);
            if (course != null)
            {
                _context.Courses.Remove(course);
                _context.SaveChanges();
            }
        }

        public IEnumerable<CourseResponseDTO> GetAllCoursesForStudent(int studentId)
        {
            var student = _context.Students.Find(studentId);

            if (student == null)
            {
                return [];
            }

            var courses = _context.Courses
                .Where(c => c.Students.Any(s => s.Id == studentId))
                .ToList();

            if (courses == null || !courses.Any())
            {
                return [];
            }
            var coursesDTOs = courses.Select(c => CourseToResponseDTO(c)).ToList();

            return coursesDTOs;
        }

    }
}
