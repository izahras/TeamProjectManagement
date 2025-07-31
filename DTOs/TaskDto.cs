using System.ComponentModel.DataAnnotations;
using TeamProjectManagement.Enums;

namespace TeamProjectManagement.DTOs
{
    public class CreateTaskDto
    {
        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;
        
        [StringLength(2000)]
        public string Description { get; set; } = string.Empty;
        
        public DateTime? DueDate { get; set; }
        
        public TaskPriority Priority { get; set; } = TaskPriority.Medium;
        
        [Range(1, 100)]
        public int Effort { get; set; } = 1;
        
        [StringLength(500)]
        public string AcceptanceCriteria { get; set; } = string.Empty;
        
        [StringLength(500)]
        public string Notes { get; set; } = string.Empty;
        
        public int? AssignedToId { get; set; }
        
        public int? EpicId { get; set; }
    }

    public class UpdateTaskDto
    {
        [StringLength(200)]
        public string? Title { get; set; }
        
        [StringLength(2000)]
        public string? Description { get; set; }
        
        public DateTime? DueDate { get; set; }
        
        public WorkItemStatus? Status { get; set; }
        
        public TaskPriority? Priority { get; set; }
        
        [Range(1, 100)]
        public int? Effort { get; set; }
        
        [StringLength(500)]
        public string? AcceptanceCriteria { get; set; }
        
        [StringLength(500)]
        public string? Notes { get; set; }
        
        public int? AssignedToId { get; set; }
        
        public int? EpicId { get; set; }
    }

    public class TaskDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime? StartedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
        public WorkItemStatus Status { get; set; }
        public TaskPriority Priority { get; set; }
        public int Effort { get; set; }
        public string AcceptanceCriteria { get; set; } = string.Empty;
        public string Notes { get; set; } = string.Empty;
        public UserDto? AssignedTo { get; set; }
        public UserDto CreatedBy { get; set; } = null!;
        public EpicDto? Epic { get; set; }
        public int CommentCount { get; set; }
        public int AttachmentCount { get; set; }
    }

    public class TaskListDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public WorkItemStatus Status { get; set; }
        public TaskPriority Priority { get; set; }
        public DateTime? DueDate { get; set; }
        public int Effort { get; set; }
        public string AssignedToName { get; set; } = string.Empty;
        public string CreatedByName { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
} 