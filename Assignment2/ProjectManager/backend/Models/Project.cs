using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ProjectManager.API.Models
{
    public class Project
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;
        
        [StringLength(500)]
        public string? Description { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public int UserId { get; set; }
        
        [JsonIgnore]
        public User? User { get; set; }
        
        public ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>();
    }
}
