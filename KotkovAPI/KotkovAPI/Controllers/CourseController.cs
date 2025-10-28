using KotkovAPI.Data.Services;
using KotkovAPI.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace KotkovAPI.Controllers{

    [ApiController]
    [Route("[controller]")]
    public class CourseController : ControllerBase
    {
        private readonly CourseService _service;

        public CourseController(CourseService service)
        {
            _service = service;
        }

        [HttpGet]
        public ActionResult<IEnumerable<CourseDTO>> GetAll()
        {
            var courses = _service.GetAll();
            if (courses == null || !courses.Any())
            {
                return NotFound();
            }
            return Ok(courses);
        }

        [HttpGet("{id}")]
        public ActionResult<CourseDTO> GetById(int id)
        {
            var course = _service.GetById(id);
            if (course == null)
            {
                return NotFound();
            }
            return Ok(course);
        }

        [HttpPost]
        public ActionResult<CourseDTO> Create(CourseDTO courseDTO)
        {
            var course = _service.Create(courseDTO);
            if (course == null)
            {
                return BadRequest("Invalid teacher ID");
            }
            return CreatedAtAction(nameof(GetById), new { id = course.Id }, courseDTO);

        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, CourseDTO courseDTO)
        {
            var course = _service.Update(id, courseDTO);
            if (course == null)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _service.Delete(id);
            return NoContent();
        }

        [HttpGet("{student_id}/courses")]
        public ActionResult<GetAllStudentsByCourseDTO> GetAllCoursesByStudentId(int student_id)
        {
            var courses = _service.GetAllCoursesForStudent(student_id);
            if (courses == null || !courses.Any())
            {
                return NotFound();
            }
            return Ok(courses);
        }
    }
}
