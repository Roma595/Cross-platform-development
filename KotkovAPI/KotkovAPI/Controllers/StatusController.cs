using KotkovAPI.Data.Services;
using KotkovAPI.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace KotkovAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StatusController : ControllerBase
    {
        private readonly StatusService _service;

        public StatusController(StatusService service)
        {
            _service = service;
        }

        [HttpGet]
        public ActionResult<IEnumerable<StatusDTO>> GetAll()
        {
            var statuses = _service.GetAll();
            if (statuses == null || !statuses.Any())
            {
                return NotFound();
            }
            return Ok(statuses);
        }

        [HttpGet("{id}")]
        public ActionResult<StatusDTO> GetById(int id)
        {
            var status = _service.GetById(id);
            if (status == null)
            {
                return NotFound();
            }
            return Ok(status);
        }

        [HttpPost]
        public ActionResult<StatusDTO> Create(StatusDTO statusDTO)
        {
            var status = _service.Create(statusDTO);
            return CreatedAtAction(nameof(GetById), new { id = status.Id }, statusDTO);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, StatusDTO statusDTO)
        {
            var status = _service.Update(id, statusDTO);
            if (status == null)
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