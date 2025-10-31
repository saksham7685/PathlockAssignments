using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ProjectManager.API.Models
{
    public class TaskItem
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;
        
        public DateTime? DueDate { get; set; }
        
        public bool IsCompleted { get; set; } = false;
        
        public int ProjectId { get; set; }
        
        [JsonIgnore]
        public Project? Project { get; set; }
    }
}
