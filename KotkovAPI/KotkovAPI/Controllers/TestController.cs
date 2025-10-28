using KotkovAPI.Data.Services;
using KotkovAPI.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace KotkovAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        private readonly TestService _service;

        public TestController(TestService service)
        {
            _service = service;
        }

        [HttpGet]
        public ActionResult<IEnumerable<TestDTO>> GetAll()
        {
            var tests = _service.GetAll();
            if (tests == null || !tests.Any())
            {
                return NotFound();
            }
            return Ok(tests);
        }

        [HttpGet("{id}")]
        public ActionResult<TestDTO> GetById(int id)
        {
            var test = _service.GetById(id);
            if (test == null)
            {
                return NotFound();
            }
            return Ok(test);
        }

        [HttpPost]
        public ActionResult<TestDTO> Create(TestDTO testDTO)
        {
            var test = _service.Create(testDTO);
            if (test == null)
            {
                return BadRequest("Invalid CourseId");
            }
            return CreatedAtAction(nameof(GetById), new { id = test.Id }, testDTO);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, TestDTO testDTO)
        {
            var test = _service.Update(id, testDTO);
            if (test == null)
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