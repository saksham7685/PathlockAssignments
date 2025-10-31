using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ProjectManager.API.Models
{
    public class User
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(50)]
        public string Username { get; set; } = string.Empty;
        
        [JsonIgnore]
        public string PasswordHash { get; set; } = string.Empty;
        
        [JsonIgnore]
        public ICollection<Project> Projects { get; set; } = new List<Project>();
    }
}
