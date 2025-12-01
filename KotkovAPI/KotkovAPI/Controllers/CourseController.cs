using KotkovAPI.Data.Services;
using KotkovAPI.DTOs;
using KotkovAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KotkovAPI.Controllers{

    [ApiController]
    [Route("api/[controller]")]
    public class CourseController : ControllerBase
    {
        private readonly CourseService _service;

        public CourseController(CourseService service)
        {
            _service = service;
        }

        [HttpGet]
        [Authorize(Roles = Roles.Admin + "," + Roles.Teacher + "," + Roles.Student)]
        public ActionResult<IEnumerable<CourseResponseDTO>> GetAll()
        {
            var courses = _service.GetAll();
            if (courses == null || !courses.Any())
            {
                return NotFound();
            }
            return Ok(courses);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = Roles.Admin + "," + Roles.Teacher + "," + Roles.Student)]
        public ActionResult<CourseResponseDTO> GetById(int id)
        {
            var course = _service.GetById(id);
            if (course == null)
            {
                return NotFound();
            }
            return Ok(course);
        }

        [HttpPost]
        [Authorize(Roles = Roles.Admin)]
        public ActionResult<CourseResponseDTO> Create(CreateCourseDTO courseDTO)
        {
            var course = _service.Create(courseDTO);
            if (course == null)
            {
                return BadRequest("Invalid teacher ID");
            }
            return CreatedAtAction(nameof(GetById), new { id = course.Id }, course);

        }

        [HttpPut("{id}")]
        [Authorize(Roles = Roles.Admin)]
        public IActionResult Update(int id, UpdateCourseDTO courseDTO)
        {
            var course = _service.Update(id, courseDTO);
            if (course == null)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = Roles.Admin)]
        public IActionResult Delete(int id)
        {
            var ok = _service.Delete(id);
            if (!ok)
            {
                return BadRequest("Unable delete Course");
            }
            return Ok();
        }

        [HttpGet("{student_id}/courses")]
        [Authorize(Roles = Roles.Admin + "," + Roles.Student)]
        public ActionResult<CourseByStudentDTO> GetAllCoursesByStudentId(int student_id)
        {
            var courses = _service.GetAllCoursesForStudent(student_id);
            if (courses == null || !courses.Any())
            {
                return BadRequest("Invalid student Id or no courses found for this student");
            }
            return Ok(courses);
        }

        [HttpGet("cource_id/average_marks")]
        [Authorize(Roles = Roles.Admin + "," + Roles.Teacher)]
        public ActionResult<StudentCourseAverageDTO> GetAvarageMarkForStudentsByCourseId(int course_id)
        {
            var courses = _service.GetAvarageMarkForStudentsByCourseId(course_id);
            if (courses == null || !courses.Any())
            {
                return BadRequest("Invalid course Id or no students found for this course");
            }
            return Ok(courses);
        }
    }
}
