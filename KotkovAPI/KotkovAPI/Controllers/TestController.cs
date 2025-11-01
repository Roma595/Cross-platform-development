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
        public ActionResult<IEnumerable<TestResponseDTO>> GetAll()
        {
            var tests = _service.GetAll();
            if (tests == null || !tests.Any())
            {
                return NotFound();
            }
            return Ok(tests);
        }

        [HttpGet("{id}")]
        public ActionResult<TestResponseDTO> GetById(int id)
        {
            var test = _service.GetById(id);
            if (test == null)
            {
                return NotFound();
            }
            return Ok(test);
        }

        [HttpPost]
        public ActionResult<TestResponseDTO> Create(CreateTestDTO testDTO)
        {
            var test = _service.Create(testDTO);
            if (test == null)
            {
                return BadRequest("Invalid CourseId");
            }
            return CreatedAtAction(nameof(GetById), new { id = test.Id }, test);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, UpdateTestDTO testDTO)
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
            var ok = _service.Delete(id);
            if (!ok)
            {
                return BadRequest("Unable delete Test");
            }
            return Ok();
        }

        [HttpGet("{course_id}/tests")]
        public ActionResult<TestResponseDTO> GetAllTestsByCourseId(int course_id)
        {
            var tests = _service.GetTestsByCourseId(course_id);
            if (tests == null || !tests.Any())
            {
                return BadRequest("Invalid course Id or no tests found for this course");
            }
            return Ok(tests);
        }

        [HttpGet("{test_id}/progresses")]
        public ActionResult<TestProgressDTO> GetAllProgressesByTestId(int test_id)
        {
            var progresses = _service.GetStudentsProgressForTest(test_id);
            if (progresses == null || !progresses.Any())
            {
                return BadRequest("Invalid test Id or no progresses found for this test");
            }
            return Ok(progresses);
        }
    }
}