using TeamProjectManagement.DTOs;
using TeamProjectManagement.Enums;

namespace TeamProjectManagement.Services.Interfaces
{
    public interface ITaskService
    {
        Task<IEnumerable<TaskListDto>> GetAllTasksAsync();
        Task<TaskDto?> GetTaskByIdAsync(int id);
        Task<TaskDto> CreateTaskAsync(CreateTaskDto createTaskDto, int createdById);
        Task<TaskDto?> UpdateTaskAsync(int id, UpdateTaskDto updateTaskDto);
        Task<bool> DeleteTaskAsync(int id);
        Task<IEnumerable<TaskListDto>> GetTasksByStatusAsync(WorkItemStatus status);
        Task<IEnumerable<TaskListDto>> GetTasksByAssigneeAsync(int assigneeId);
        Task<IEnumerable<TaskListDto>> GetTasksByEpicAsync(int epicId);
        Task<bool> AssignTaskAsync(int taskId, int assigneeId);
        Task<bool> UpdateTaskStatusAsync(int taskId, WorkItemStatus status);
        Task<IEnumerable<TaskListDto>> GetTasksByPriorityAsync(TaskPriority priority);
    }
}