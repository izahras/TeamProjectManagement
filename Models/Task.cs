using System.ComponentModel.DataAnnotations;
using TeamProjectManagement.Enums;

namespace TeamProjectManagement.Models
{
    public class WorkItem
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;
        
        [StringLength(2000)]
        public string Description { get; set; } = string.Empty;
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime? DueDate { get; set; }
        
        public DateTime? StartedAt { get; set; }
        
        public DateTime? CompletedAt { get; set; }
        
        public WorkItemStatus Status { get; set; } = WorkItemStatus.ToDo;
        
        public TaskPriority Priority { get; set; } = TaskPriority.Medium;
        
        [Range(1, 100)]
        public int Effort { get; set; } = 1; // Story points or hours
        
        [StringLength(500)]
        public string AcceptanceCriteria { get; set; } = string.Empty;
        
        [StringLength(500)]
        public string Notes { get; set; } = string.Empty;
        
        // Foreign keys
        public int CreatedById { get; set; }
        public virtual User CreatedBy { get; set; } = null!;
        
        public int? AssignedToId { get; set; }
        public virtual User? AssignedTo { get; set; }
        
        public int? EpicId { get; set; }
        public virtual Epic? Epic { get; set; }
        
        // Navigation properties
        public virtual ICollection<WorkItemComment> Comments { get; set; } = new List<WorkItemComment>();
        public virtual ICollection<WorkItemAttachment> Attachments { get; set; } = new List<WorkItemAttachment>();
    }
} 