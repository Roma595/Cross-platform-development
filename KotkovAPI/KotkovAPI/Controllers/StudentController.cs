using System.Xml.Schema;
using KotkovAPI.Data.Services;
using KotkovAPI.DTOs;
using KotkovAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace KotkovAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StudentController : ControllerBase
    {
        private readonly StudentService _service;

        public StudentController(StudentService service)
        {
            _service = service;
        }

        [HttpGet]
        public ActionResult<IEnumerable<StudentDTO>> GetAll()
        {
            var students = _service.GetAll();
            if (students == null || !students.Any())
            {
                return NotFound();
            }
            return Ok(students);
        }

        [HttpGet("{id}")]
        public ActionResult<StudentDTO> GetById(int id)
        {
            var student = _service.GetById(id);
            if (student == null)
            {
                return NotFound();
            }
            return Ok(student);
        }

        [HttpPost]
        public ActionResult<StudentDTO> Create(StudentDTO studentDTO)
        {
            var student = _service.Create(studentDTO);
            return CreatedAtAction(nameof(GetById), new { id = student.Id }, studentDTO);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, StudentDTO studentDTO)
        {
            var student = _service.Update(id, studentDTO);
            if (student == null)
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

        

        [HttpGet("{course_id}/students")]
        public ActionResult<GetAllStudentsByCourseDTO> GetAllStudentsByCourseId(int course_id)
        {
            var students = _service.GetAllStudentsByCourse(course_id);
            if (students == null || !students.Any())
            {
                return NotFound();
            }
            return Ok(students);
        }

        [HttpPost("push")]
        public ActionResult<AttendenceResponseDTO> PushStudentToCourse(PushStudentToCourseDTO pushStudentToCourseDTO)
        {
            var attendence = _service.PushStudentToCourse(pushStudentToCourseDTO.StudentId, pushStudentToCourseDTO.CourseId, pushStudentToCourseDTO.StatusId);
            if (attendence == null)
            {
                return NotFound();
            }
            return Ok(attendence);
        }
    }
}