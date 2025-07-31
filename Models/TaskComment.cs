using System.ComponentModel.DataAnnotations;

namespace TeamProjectManagement.Models
{
    public class WorkItemComment
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(2000)]
        public string Content { get; set; } = string.Empty;
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime? UpdatedAt { get; set; }
        
        // Foreign keys
        public int TaskId { get; set; }
        public virtual WorkItem Task { get; set; } = null!;
        
        public int CreatedById { get; set; }
        public virtual User CreatedBy { get; set; } = null!;
    }
} 