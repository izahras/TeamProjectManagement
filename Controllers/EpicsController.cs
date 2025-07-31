using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TeamProjectManagement.DTOs;
using TeamProjectManagement.Enums;
using TeamProjectManagement.Attributes;
using TeamProjectManagement.Services.Interfaces;

namespace TeamProjectManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [AuthorizeRoles(UserRole.TeamLead, UserRole.ProjectManager, UserRole.ProductOwner, UserRole.ScrumMaster)]
    public class EpicsController : ControllerBase
    {
        private readonly IEpicService _epicService;
        private readonly IJwtService _jwtService;

        public EpicsController(IEpicService epicService, IJwtService jwtService)
        {
            _epicService = epicService;
            _jwtService = jwtService;
        }

        private int GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
                throw new UnauthorizedAccessException("Invalid user token");
            
            return userId;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EpicListDto>>> GetEpics()
        {
            var epics = await _epicService.GetAllEpicsAsync();
            return Ok(epics);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EpicDto>> GetEpic(int id)
        {
            var epic = await _epicService.GetEpicByIdAsync(id);
            if (epic == null)
                return NotFound();

            return Ok(epic);
        }

        [HttpPost]
        [AuthorizeRoles(UserRole.ProjectManager, UserRole.ProductOwner)]
        public async Task<ActionResult<EpicDto>> CreateEpic(CreateEpicDto createEpicDto)
        {
            int currentUserId = GetCurrentUserId();
            var epic = await _epicService.CreateEpicAsync(createEpicDto, currentUserId);
            return CreatedAtAction(nameof(GetEpic), new { id = epic.Id }, epic);
        }

        [HttpPut("{id}")]
        [AuthorizeRoles(UserRole.ProjectManager, UserRole.ProductOwner)]
        public async Task<ActionResult<EpicDto>> UpdateEpic(int id, UpdateEpicDto updateEpicDto)
        {
            var epic = await _epicService.UpdateEpicAsync(id, updateEpicDto);
            if (epic == null)
                return NotFound();

            return Ok(epic);
        }

        [HttpDelete("{id}")]
        [AuthorizeRoles(UserRole.ProjectManager, UserRole.ProductOwner)]
        public async Task<ActionResult> DeleteEpic(int id)
        {
            var result = await _epicService.DeleteEpicAsync(id);
            if (!result)
                return NotFound();

            return NoContent();
        }

        [HttpGet("status/{status}")]
        public async Task<ActionResult<IEnumerable<EpicListDto>>> GetEpicsByStatus(WorkItemStatus status)
        {
            var epics = await _epicService.GetEpicsByStatusAsync(status);
            return Ok(epics);
        }

        [HttpGet("creator/{creatorId}")]
        public async Task<ActionResult<IEnumerable<EpicListDto>>> GetEpicsByCreator(int creatorId)
        {
            var epics = await _epicService.GetEpicsByCreatorAsync(creatorId);
            return Ok(epics);
        }

        [HttpGet("priority/{priority}")]
        public async Task<ActionResult<IEnumerable<EpicListDto>>> GetEpicsByPriority(TaskPriority priority)
        {
            var epics = await _epicService.GetEpicsByPriorityAsync(priority);
            return Ok(epics);
        }
    }
} 