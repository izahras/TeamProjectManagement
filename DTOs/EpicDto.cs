using System.ComponentModel.DataAnnotations;
using TeamProjectManagement.Enums;

namespace TeamProjectManagement.DTOs
{
    public class CreateEpicDto
    {
        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;
        
        [StringLength(1000)]
        public string Description { get; set; } = string.Empty;
        
        public DateTime? DueDate { get; set; }
        
        public TaskPriority Priority { get; set; } = TaskPriority.Medium;
    }

    public class UpdateEpicDto
    {
        [StringLength(200)]
        public string? Title { get; set; }
        
        [StringLength(1000)]
        public string? Description { get; set; }
        
        public DateTime? DueDate { get; set; }
        
        public WorkItemStatus? Status { get; set; }
        
        public TaskPriority? Priority { get; set; }
    }

    public class EpicDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? DueDate { get; set; }
        public WorkItemStatus Status { get; set; }
        public TaskPriority Priority { get; set; }
        public UserDto CreatedBy { get; set; } = null!;
        public int TaskCount { get; set; }
        public int CompletedTaskCount { get; set; }
    }

    public class EpicListDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public WorkItemStatus Status { get; set; }
        public TaskPriority Priority { get; set; }
        public DateTime? DueDate { get; set; }
        public string CreatedByName { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public int TaskCount { get; set; }
        public int CompletedTaskCount { get; set; }
    }
} 