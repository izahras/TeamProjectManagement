using Microsoft.EntityFrameworkCore;
using AutoMapper;
using TeamProjectManagement.Data;
using TeamProjectManagement.DTOs;
using TeamProjectManagement.Enums;
using TeamProjectManagement.Models;
using TeamProjectManagement.Services.Interfaces;

namespace TeamProjectManagement.Services.Repositories
{
    public class TaskService : ITaskService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public TaskService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TaskListDto>> GetAllTasksAsync()
        {
            var tasks = await _context.Tasks
                .Include(t => t.AssignedTo)
                .Include(t => t.CreatedBy)
                .ToListAsync();

            return _mapper.Map<IEnumerable<TaskListDto>>(tasks);
        }

        public async Task<TaskDto?> GetTaskByIdAsync(int id)
        {
            var task = await _context.Tasks
                .Include(t => t.AssignedTo)
                .Include(t => t.CreatedBy)
                .Include(t => t.Epic)
                .Include(t => t.Comments)
                .Include(t => t.Attachments)
                .FirstOrDefaultAsync(t => t.Id == id);

            return _mapper.Map<TaskDto>(task);
        }

        public async Task<TaskDto> CreateTaskAsync(CreateTaskDto createTaskDto, int createdById)
        {
            var task = _mapper.Map<WorkItem>(createTaskDto);
            task.CreatedById = createdById;

            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();

            return await GetTaskByIdAsync(task.Id) ?? throw new InvalidOperationException("Failed to create task");
        }

        public async Task<TaskDto?> UpdateTaskAsync(int id, UpdateTaskDto updateTaskDto)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null)
                return null;

            if (updateTaskDto.Title != null)
                task.Title = updateTaskDto.Title;

            if (updateTaskDto.Description != null)
                task.Description = updateTaskDto.Description;

            if (updateTaskDto.DueDate.HasValue)
                task.DueDate = updateTaskDto.DueDate;

            if (updateTaskDto.Status.HasValue)
            {
                task.Status = updateTaskDto.Status.Value;

                if (updateTaskDto.Status.Value == WorkItemStatus.InProgress && task.StartedAt == null)
                    task.StartedAt = DateTime.UtcNow;
                else if (updateTaskDto.Status.Value == WorkItemStatus.Done && task.CompletedAt == null)
                    task.CompletedAt = DateTime.UtcNow;
            }

            if (updateTaskDto.Priority.HasValue)
                task.Priority = updateTaskDto.Priority.Value;

            if (updateTaskDto.Effort.HasValue)
                task.Effort = updateTaskDto.Effort.Value;

            if (updateTaskDto.AcceptanceCriteria != null)
                task.AcceptanceCriteria = updateTaskDto.AcceptanceCriteria;

            if (updateTaskDto.Notes != null)
                task.Notes = updateTaskDto.Notes;

            if (updateTaskDto.AssignedToId.HasValue)
                task.AssignedToId = updateTaskDto.AssignedToId.Value;

            if (updateTaskDto.EpicId.HasValue)
                task.EpicId = updateTaskDto.EpicId.Value;

            await _context.SaveChangesAsync();

            return await GetTaskByIdAsync(task.Id);
        }

        public async Task<bool> DeleteTaskAsync(int id)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null)
                return false;

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<TaskListDto>> GetTasksByStatusAsync(WorkItemStatus status)
        {
            var tasks = await _context.Tasks
                .Include(t => t.AssignedTo)
                .Include(t => t.CreatedBy)
                .Where(t => t.Status == status)
                .ToListAsync();

            return _mapper.Map<IEnumerable<TaskListDto>>(tasks);
        }

        public async Task<IEnumerable<TaskListDto>> GetTasksByAssigneeAsync(int assigneeId)
        {
            var tasks = await _context.Tasks
                .Include(t => t.AssignedTo)
                .Include(t => t.CreatedBy)
                .Where(t => t.AssignedToId == assigneeId)
                .ToListAsync();

            return _mapper.Map<IEnumerable<TaskListDto>>(tasks);
        }

        public async Task<IEnumerable<TaskListDto>> GetTasksByEpicAsync(int epicId)
        {
            var tasks = await _context.Tasks
                .Include(t => t.AssignedTo)
                .Include(t => t.CreatedBy)
                .Where(t => t.EpicId == epicId)
                .ToListAsync();

            return _mapper.Map<IEnumerable<TaskListDto>>(tasks);
        }

        public async Task<bool> AssignTaskAsync(int taskId, int assigneeId)
        {
            var task = await _context.Tasks.FindAsync(taskId);
            if (task == null)
                return false;

            var user = await _context.Users.FindAsync(assigneeId);
            if (user == null)
                return false;

            task.AssignedToId = assigneeId;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateTaskStatusAsync(int taskId, WorkItemStatus status)
        {
            var task = await _context.Tasks.FindAsync(taskId);
            if (task == null)
                return false;

            task.Status = status;

            if (status == WorkItemStatus.InProgress && task.StartedAt == null)
                task.StartedAt = DateTime.UtcNow;
            else if (status == WorkItemStatus.Done && task.CompletedAt == null)
                task.CompletedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<TaskListDto>> GetTasksByPriorityAsync(TaskPriority priority)
        {
            var tasks = await _context.Tasks
                .Include(t => t.AssignedTo)
                .Include(t => t.CreatedBy)
                .Where(t => t.Priority == priority)
                .ToListAsync();

            return _mapper.Map<IEnumerable<TaskListDto>>(tasks);
        }
    }
}