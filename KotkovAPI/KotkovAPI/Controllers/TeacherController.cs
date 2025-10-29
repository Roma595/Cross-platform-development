namespace KotkovAPI.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using KotkovAPI.Data;
    using KotkovAPI.Models;
    using KotkovAPI.Data.Services;
    using KotkovAPI.DTOs;
    using Microsoft.EntityFrameworkCore;

    [ApiController]
    [Route("[controller]")]
    public class TeacherController : ControllerBase
    {
        private readonly TeacherService _service;

        public TeacherController(TeacherService service)
        {
            _service = service;
        }

        [HttpGet]
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
        public ActionResult<TeacherResponseDTO> Create(CreateTeacherDTO teacherDTO)
        {
            var teacher = _service.Create(teacherDTO);
            return CreatedAtAction(nameof(GetById), new { id = teacher.Id }, teacherDTO);
        }

        [HttpPut("{id}")]
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
        public IActionResult Delete(int id)
        {
            _service.Delete(id);
            return NoContent();
        }

       
        
    }
}