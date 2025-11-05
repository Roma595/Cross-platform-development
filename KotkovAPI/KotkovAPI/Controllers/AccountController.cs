using KotkovAPI.Data.Services;
using KotkovAPI.DTOs;
using KotkovAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KotkovAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly AccountService _service;

        public AccountController(AccountService service)
        {
            _service = service;
        }

        [HttpPost("token")]
        public IActionResult Token(AccountDTO accountDTO)
        {
            var response = _service.Token(accountDTO);
            if (response == null)
            {
                return BadRequest("Invalid username or password.");
            }
            return Ok(response);
        }


        [HttpPost("reg")]
        [Authorize(Roles = Roles.Admin)]
        public IActionResult Register(RegisterPersonDTO registerPersonDTO)
        {
            var ok = _service.RegisterPerson(registerPersonDTO);
            if (!ok)
            {
                return BadRequest("Person is already exist or invalid Role");
            }
            return Ok();
        }
    }
}