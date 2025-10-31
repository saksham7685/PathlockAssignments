using TaskManagerAPI.Models;

namespace TaskManagerAPI.Repositories;

public interface ITaskRepository
{
    Task<IEnumerable<TaskItem>> GetAllTasksAsync();
    Task<TaskItem?> GetTaskByIdAsync(Guid id);
    Task<TaskItem> AddTaskAsync(TaskItem task);
    Task<TaskItem?> UpdateTaskAsync(TaskItem task);
    Task<bool> DeleteTaskAsync(Guid id);
}
