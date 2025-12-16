using KotkovAPI.Data.Services;
using KotkovAPI.DTOs;
using KotkovAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KotkovAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentController : ControllerBase
    {
        private readonly StudentService _service;

        public StudentController(StudentService service)
        {
            _service = service;
        }

        [HttpGet]
        [Authorize(Roles = Roles.Admin)]
        public ActionResult<IEnumerable<StudentResponseDTO>> GetAll()
        {
            var students = _service.GetAll();
            if (students == null || !students.Any())
            {
                return NotFound();
            }
            return Ok(students);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = Roles.Admin + "," + Roles.Teacher + "," + Roles.Student)]
        public ActionResult<StudentResponseDTO> GetById(int id)
        {
            var student = _service.GetById(id);
            if (student == null)
            {
                return NotFound();
            }
            return Ok(student);
        }

        [HttpPost]
        [Authorize(Roles = Roles.Admin)]
        public ActionResult<StudentResponseDTO> Create(CreateStudentDTO studentDTO)
        {
            var student = _service.Create(studentDTO);
            return CreatedAtAction(nameof(GetById), new { id = student.Id }, student);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = Roles.Admin)]
        public IActionResult Update(int id, UpdateStudentDTO studentDTO)
        {
            var student = _service.Update(id, studentDTO);
            if (student == null)
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
                return BadRequest("Unable delete Student");
            }
            return Ok();
        }


        [HttpGet("{course_id}/students")]
        [Authorize(Roles = Roles.Admin + "," + Roles.Teacher + "," + Roles.Student)]
        public ActionResult<StudentByCourseDTO> GetAllStudentsByCourseId(int course_id)
        {
            var students = _service.GetAllStudentsByCourse(course_id);
            if (students == null || !students.Any())
            {
                return BadRequest("Invalid course Id or no students found for this course");
            }
            return Ok(students);
        }

        [HttpPost("add_to_course")]
        [Authorize(Roles = Roles.Admin)]
        public ActionResult<AttendenceResponseDTO> AddStudentToCourse(AddStudentToCourseDTO attendenceDTO)
        {
            var attendence = _service.AddStudentToCourse(attendenceDTO);
            if (attendence == null)
            {
                return BadRequest("Invalid student ID or course ID, or student is already on the course");
            }
            return Ok(attendence);
        }

        [HttpPost("add_progress")]
        [Authorize(Roles = Roles.Teacher)]
        public ActionResult<ProgressResponseDTO> AddStudentProgressForTest(AddStudentProgressForTestDTO progressDTO)
        {
            var progress = _service.AddStudentProgressForTest(progressDTO);
            if (progress == null)
            {
                return BadRequest("Invalid student ID or test ID, or student has already received a mark for this test");
            }
            return Ok(progress);
        }

        [HttpGet("{student_id}/progresses")]
        [Authorize(Roles = Roles.Admin + "," + Roles.Teacher + "," + Roles.Student)]
        public ActionResult<ProgressResponseDTO> GetAllProgressesByStudentId(int student_id)
        {
            var progresses = _service.GetAllProgressesForStudent(student_id);
            if (progresses == null || !progresses.Any())
            {
                return BadRequest("Invalid student Id or no progresses found for this student");
            }
            return Ok(progresses);
        }
    }
}