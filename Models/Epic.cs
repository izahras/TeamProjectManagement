using System.ComponentModel.DataAnnotations;
using TeamProjectManagement.Enums;

namespace TeamProjectManagement.Models
{
    public class Epic
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;
        
        [StringLength(1000)]
        public string Description { get; set; } = string.Empty;
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime? DueDate { get; set; }
        
        public WorkItemStatus Status { get; set; } = WorkItemStatus.ToDo;
        
        public TaskPriority Priority { get; set; } = TaskPriority.Medium;
        
        public int CreatedById { get; set; }
        public virtual User CreatedBy { get; set; } = null!;
        
        // Navigation properties
        public virtual ICollection<WorkItem> Tasks { get; set; } = new List<WorkItem>();
    }
} 