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
    [AuthorizeRoles(UserRole.Developer, UserRole.TeamLead, UserRole.ProjectManager, UserRole.ProductOwner, UserRole.ScrumMaster, UserRole.Tester)]
    public class TasksController : ControllerBase
    {
        private readonly ITaskService _taskService;
        private readonly IJwtService _jwtService;

        public TasksController(ITaskService taskService, IJwtService jwtService)
        {
            _taskService = taskService;
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
        public async Task<ActionResult<IEnumerable<TaskListDto>>> GetTasks()
        {
            var tasks = await _taskService.GetAllTasksAsync();
            return Ok(tasks);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TaskDto>> GetTask(int id)
        {
            var task = await _taskService.GetTaskByIdAsync(id);
            if (task == null)
                return NotFound();

            return Ok(task);
        }

        [HttpPost]
        [AuthorizeRoles(UserRole.TeamLead, UserRole.ProjectManager, UserRole.ProductOwner, UserRole.ScrumMaster)]
        public async Task<ActionResult<TaskDto>> CreateTask(CreateTaskDto createTaskDto)
        {
            int currentUserId = GetCurrentUserId();
            var task = await _taskService.CreateTaskAsync(createTaskDto, currentUserId);
            return CreatedAtAction(nameof(GetTask), new { id = task.Id }, task);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<TaskDto>> UpdateTask(int id, UpdateTaskDto updateTaskDto)
        {
            var task = await _taskService.UpdateTaskAsync(id, updateTaskDto);
            if (task == null)
                return NotFound();

            return Ok(task);
        }

        [HttpDelete("{id}")]
        [AuthorizeRoles(UserRole.TeamLead, UserRole.ProjectManager, UserRole.ProductOwner, UserRole.ScrumMaster)]
        public async Task<ActionResult> DeleteTask(int id)
        {
            var result = await _taskService.DeleteTaskAsync(id);
            if (!result)
                return NotFound();

            return NoContent();
        }

        [HttpGet("status/{status}")]
        public async Task<ActionResult<IEnumerable<TaskListDto>>> GetTasksByStatus(WorkItemStatus status)
        {
            var tasks = await _taskService.GetTasksByStatusAsync(status);
            return Ok(tasks);
        }

        [HttpGet("assignee/{assigneeId}")]
        public async Task<ActionResult<IEnumerable<TaskListDto>>> GetTasksByAssignee(int assigneeId)
        {
            var tasks = await _taskService.GetTasksByAssigneeAsync(assigneeId);
            return Ok(tasks);
        }

        [HttpGet("epic/{epicId}")]
        public async Task<ActionResult<IEnumerable<TaskListDto>>> GetTasksByEpic(int epicId)
        {
            var tasks = await _taskService.GetTasksByEpicAsync(epicId);
            return Ok(tasks);
        }

        [HttpPost("{id}/assign/{assigneeId}")]
        [AuthorizeRoles(UserRole.TeamLead, UserRole.ProjectManager, UserRole.ProductOwner, UserRole.ScrumMaster)]
        public async Task<ActionResult> AssignTask(int id, int assigneeId)
        {
            var result = await _taskService.AssignTaskAsync(id, assigneeId);
            if (!result)
                return NotFound();

            return NoContent();
        }

        [HttpPost("{id}/status/{status}")]
        public async Task<ActionResult> UpdateTaskStatus(int id, WorkItemStatus status)
        {
            var result = await _taskService.UpdateTaskStatusAsync(id, status);
            if (!result)
                return NotFound();

            return NoContent();
        }

        [HttpGet("priority/{priority}")]
        public async Task<ActionResult<IEnumerable<TaskListDto>>> GetTasksByPriority(TaskPriority priority)
        {
            var tasks = await _taskService.GetTasksByPriorityAsync(priority);
            return Ok(tasks);
        }
    }
} 