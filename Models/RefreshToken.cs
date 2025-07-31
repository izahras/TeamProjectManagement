using System.ComponentModel.DataAnnotations;

namespace TeamProjectManagement.Models
{
    public class RefreshToken
    {
        public int Id { get; set; }
        
        [Required]
        public string Token { get; set; } = string.Empty;
        
        public DateTime ExpiresAt { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public bool IsRevoked { get; set; } = false;
        
        public DateTime? RevokedAt { get; set; }
        
        public string? RevokedBy { get; set; }
        
        public string? ReplacedByToken { get; set; }
        
        public string? ReasonRevoked { get; set; }
        
        // Foreign key
        public int UserId { get; set; }
        public virtual User User { get; set; } = null!;
    }
} 