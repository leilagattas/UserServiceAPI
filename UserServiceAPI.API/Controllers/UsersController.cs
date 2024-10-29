using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using UserServiceAPI.Common.Models;
using UserServiceAPI.Domain.Models;
using UserServiceAPI.Services.DTO;
using UserServiceAPI.Services.Interfaces;

namespace UserServiceAPI.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _service;

        public UsersController(IUserService service)
        {
            _service = service;
        }

        // POST: api/v1/Users
        [HttpPost]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<UserDto>> PostUser(UserCreateDto userDto)
        {
            UserDto user = await _service.Create(userDto);

            return CreatedAtAction(nameof(GetActiveUsers), new { id = user.Id }, user);
        }

        // GET: api/v1/Users/active
        [HttpGet("active")]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IAsyncEnumerable<User>>> GetActiveUsers()
        {
            var users = await _service.GetAllActive();

            return Ok(users);
        }

        // PATCH: api/v1/Users/2c4f92ec-a750-4e84-beae-7cf30fe42d6b/active
        [HttpPatch("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<User>> ToggleUserActiveStatus(Guid id)
        {
            User user = await _service.UpdateActive(id);

            return Ok(user);
        }

        // DELETE: api/v1/Users/2c4f92ec-a750-4e84-beae-7cf30fe42d6b
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            await _service.Delete(id);
            return NoContent();
        }
    }
}
