using Microsoft.AspNetCore.Mvc;
using TeamProjectManagement.DTOs;
using TeamProjectManagement.Attributes;
using TeamProjectManagement.Enums;
using TeamProjectManagement.Services.Interfaces;

namespace TeamProjectManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [AuthorizeRoles(UserRole.ProjectManager, UserRole.TeamLead, UserRole.ProductOwner)]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetUser(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
                return NotFound();

            return Ok(user);
        }

        [HttpPost]
        [AuthorizeRoles(UserRole.ProjectManager, UserRole.ProductOwner)]
        public async Task<ActionResult<UserDto>> CreateUser(CreateUserDto createUserDto)
        {
            try
            {
                var user = await _userService.CreateUserAsync(createUserDto);
                return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        [AuthorizeRoles(UserRole.ProjectManager, UserRole.ProductOwner)]
        public async Task<ActionResult<UserDto>> UpdateUser(int id, UpdateUserDto updateUserDto)
        {
            try
            {
                var user = await _userService.UpdateUserAsync(id, updateUserDto);
                if (user == null)
                    return NotFound();

                return Ok(user);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        [AuthorizeRoles(UserRole.ProjectManager, UserRole.ProductOwner)]
        public async Task<ActionResult> DeleteUser(int id)
        {
            var result = await _userService.DeleteUserAsync(id);
            if (!result)
                return NotFound();

            return NoContent();
        }

        [HttpGet("role/{role}")]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUsersByRole(int role)
        {
            var users = await _userService.GetUsersByRoleAsync(role);
            return Ok(users);
        }

        [HttpPost("{id}/deactivate")]
        [AuthorizeRoles(UserRole.ProjectManager, UserRole.ProductOwner)]
        public async Task<ActionResult> DeactivateUser(int id)
        {
            var result = await _userService.DeactivateUserAsync(id);
            if (!result)
                return NotFound();

            return NoContent();
        }

        [HttpPost("{id}/activate")]
        [AuthorizeRoles(UserRole.ProjectManager, UserRole.ProductOwner)]
        public async Task<ActionResult> ActivateUser(int id)
        {
            var result = await _userService.ActivateUserAsync(id);
            if (!result)
                return NotFound();

            return NoContent();
        }
    }
} 