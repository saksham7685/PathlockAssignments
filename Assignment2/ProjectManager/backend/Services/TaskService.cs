using Microsoft.EntityFrameworkCore;
using ProjectManager.API.Data;
using ProjectManager.API.DTOs;
using ProjectManager.API.Models;

namespace ProjectManager.API.Services
{
    public interface ITaskService
    {
        Task<TaskItem> AddTaskToProject(int projectId, CreateTaskDto taskDto, int userId);
        Task<bool> UpdateTask(int taskId, UpdateTaskDto taskDto, int userId);
        Task<bool> DeleteTask(int taskId, int userId);
        Task<bool> ToggleTaskCompletion(int taskId, int userId);
    }

    public class TaskService : ITaskService
    {
        private readonly AppDbContext _context;

        public TaskService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<TaskItem> AddTaskToProject(int projectId, CreateTaskDto taskDto, int userId)
        {
            var project = await _context.Projects
                .FirstOrDefaultAsync(p => p.Id == projectId && p.UserId == userId);

            if (project == null)
            {
                throw new KeyNotFoundException("Project not found");
            }

            var task = new TaskItem
            {
                Title = taskDto.Title,
                DueDate = taskDto.DueDate,
                ProjectId = projectId
            };

            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();
            return task;
        }

        public async Task<bool> UpdateTask(int taskId, UpdateTaskDto taskDto, int userId)
        {
            var task = await _context.Tasks
                .Include(t => t.Project)
                .FirstOrDefaultAsync(t => t.Id == taskId && t.Project!.UserId == userId);

            if (task == null)
            {
                return false;
            }

            if (taskDto.Title != null)
            {
                task.Title = taskDto.Title;
            }

            if (taskDto.DueDate.HasValue)
            {
                task.DueDate = taskDto.DueDate;
            }

            if (taskDto.IsCompleted.HasValue)
            {
                task.IsCompleted = taskDto.IsCompleted.Value;
            }

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteTask(int taskId, int userId)
        {
            var task = await _context.Tasks
                .Include(t => t.Project)
                .FirstOrDefaultAsync(t => t.Id == taskId && t.Project!.UserId == userId);

            if (task == null)
            {
                return false;
            }

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ToggleTaskCompletion(int taskId, int userId)
        {
            var task = await _context.Tasks
                .Include(t => t.Project)
                .FirstOrDefaultAsync(t => t.Id == taskId && t.Project!.UserId == userId);

            if (task == null)
            {
                return false;
            }

            task.IsCompleted = !task.IsCompleted;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
