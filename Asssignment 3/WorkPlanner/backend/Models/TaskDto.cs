namespace WorkPlanner.API.Models;

public class TaskDto
{
    public string Title { get; set; } = string.Empty;
    public int EstimatedHours { get; set; }
    public DateTime DueDate { get; set; }
    public List<string> Dependencies { get; set; } = new();
}

public class ScheduleRequest
{
    public List<TaskDto> Tasks { get; set; } = new();
}

public class ScheduleResponse
{
    public List<string> RecommendedOrder { get; set; } = new();
    public string? Error { get; set; }
}
