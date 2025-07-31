using System.ComponentModel.DataAnnotations;

namespace TeamProjectManagement.Models
{
    public class WorkItemAttachment
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(255)]
        public string FileName { get; set; } = string.Empty;
        
        [Required]
        [StringLength(100)]
        public string FileType { get; set; } = string.Empty;
        
        [Required]
        public string FilePath { get; set; } = string.Empty;
        
        public long FileSize { get; set; }
        
        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
        
        // Foreign keys
        public int TaskId { get; set; }
        public virtual WorkItem Task { get; set; } = null!;
        
        public int UploadedById { get; set; }
        public virtual User UploadedBy { get; set; } = null!;
    }
} 