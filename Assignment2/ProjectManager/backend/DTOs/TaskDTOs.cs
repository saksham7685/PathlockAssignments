using System.ComponentModel.DataAnnotations;

namespace ProjectManager.API.DTOs
{
    public class CreateTaskDto
    {
        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;
        
        public DateTime? DueDate { get; set; }
    }

    public class UpdateTaskDto
    {
        [StringLength(200)]
        public string? Title { get; set; }
        
        public DateTime? DueDate { get; set; }
        
        public bool? IsCompleted { get; set; }
    }

    public class TaskDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public DateTime? DueDate { get; set; }
        public bool IsCompleted { get; set; }
        public int ProjectId { get; set; }
    }
}
