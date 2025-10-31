using System.ComponentModel.DataAnnotations;

namespace ProjectManager.API.DTOs
{
    public class CreateProjectDto
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;
        
        [StringLength(500)]
        public string? Description { get; set; }
    }

    public class ProjectDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public int TaskCount { get; set; }
        public int CompletedTaskCount { get; set; }
    }

    public class ProjectDetailDto : ProjectDto
    {
        public ICollection<TaskDto> Tasks { get; set; } = new List<TaskDto>();
    }
}
