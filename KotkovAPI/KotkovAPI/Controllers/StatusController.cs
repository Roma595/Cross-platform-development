using KotkovAPI.Data.Services;
using KotkovAPI.DTOs;
using KotkovAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KotkovAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StatusController : ControllerBase
    {
        private readonly StatusService _service;

        public StatusController(StatusService service)
        {
            _service = service;
        }

        [HttpGet]
        [Authorize(Roles = Roles.Admin)]
        public ActionResult<IEnumerable<StatusResponseDTO>> GetAll()
        {
            var statuses = _service.GetAll();
            if (statuses == null || !statuses.Any())
            {
                return NotFound();
            }
            return Ok(statuses);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = Roles.Admin + "," + Roles.Student)]
        public ActionResult<StatusResponseDTO> GetById(int id)
        {
            var status = _service.GetById(id);
            if (status == null)
            {
                return NotFound();
            }
            return Ok(status);
        }

        [HttpPost]
        [Authorize(Roles = Roles.Admin)]
        public ActionResult<StatusResponseDTO> Create(CreateStatusDTO statusDTO)
        {
            var status = _service.Create(statusDTO);
            return CreatedAtAction(nameof(GetById), new { id = status.Id }, status);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = Roles.Admin)]
        public IActionResult Update(int id, UpdateStatusDTO statusDTO)
        {
            var status = _service.Update(id, statusDTO);
            if (status == null)
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
                return BadRequest("Unable delete Status");
            }
            return Ok();
        }

        
    }
}