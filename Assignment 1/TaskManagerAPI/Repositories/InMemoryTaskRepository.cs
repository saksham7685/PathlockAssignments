using TaskManagerAPI.Models;

namespace TaskManagerAPI.Repositories;

public class InMemoryTaskRepository : ITaskRepository
{
    private readonly Dictionary<Guid, TaskItem> _tasks = new();

    public async Task<IEnumerable<TaskItem>> GetAllTasksAsync()
    {
        return await Task.FromResult(_tasks.Values);
    }

    public async Task<TaskItem?> GetTaskByIdAsync(Guid id)
    {
        _tasks.TryGetValue(id, out var task);
        return await Task.FromResult(task);
    }

    public async Task<TaskItem> AddTaskAsync(TaskItem task)
    {
        task.Id = Guid.NewGuid();
        _tasks[task.Id] = task;
        return await Task.FromResult(task);
    }

    public async Task<TaskItem?> UpdateTaskAsync(TaskItem task)
    {
        if (!_tasks.ContainsKey(task.Id))
            return null;

        _tasks[task.Id] = task;
        return await Task.FromResult(task);
    }

    public async Task<bool> DeleteTaskAsync(Guid id)
    {
        return await Task.FromResult(_tasks.Remove(id));
    }
}
