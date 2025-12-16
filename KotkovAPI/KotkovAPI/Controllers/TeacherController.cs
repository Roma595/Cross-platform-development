namespace KotkovAPI.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using KotkovAPI.Data.Services;
    using KotkovAPI.DTOs;
    using Microsoft.AspNetCore.Authorization;
    using KotkovAPI.Models;

    [ApiController]
    [Route("api/[controller]")]
    public class TeacherController : ControllerBase
    {
        private readonly TeacherService _service;

        public TeacherController(TeacherService service)
        {
            _service = service;
        }

        [HttpGet]
        [Authorize(Roles = Roles.Admin)]
        public ActionResult<IEnumerable<TeacherResponseDTO>> GetAll()
        {
            var teachers = _service.GetAll();
            if (teachers == null || !teachers.Any())
            {
                return NotFound();
            }
            return Ok(teachers);
        }
        [HttpGet("{id}")]
        [Authorize(Roles = Roles.Admin + "," + Roles.Student + "," + Roles.Teacher)]
        public ActionResult<TeacherResponseDTO> GetById(int id)
        {
            var teacher = _service.GetById(id);
            if (teacher == null)
            {
                return NotFound();
            }
            return Ok(teacher);
        }

        [HttpPost]
        [Authorize(Roles = Roles.Admin)]
        public ActionResult<TeacherResponseDTO> Create(CreateTeacherDTO teacherDTO)
        {
            var teacher = _service.Create(teacherDTO);
            return CreatedAtAction(nameof(GetById), new { id = teacher.Id }, teacher);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = Roles.Admin)]
        public IActionResult Update(int id, UpdateTeacherDTO teacherDTO)
        {
            var teacher = _service.Update(id, teacherDTO);
            if (teacher == null)
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
                return BadRequest("Unable delete Teacher");
            }
            return Ok();
        }

       
        
    }
}