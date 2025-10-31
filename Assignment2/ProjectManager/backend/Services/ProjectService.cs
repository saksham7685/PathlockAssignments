using Microsoft.EntityFrameworkCore;
using ProjectManager.API.Data;
using ProjectManager.API.DTOs;
using ProjectManager.API.Models;
using ProjectManager.API.Services;

namespace ProjectManager.API.Services
{
    public interface IProjectService
    {
        Task<IEnumerable<ProjectDto>> GetProjectsForUser(int userId);
        Task<ProjectDetailDto> GetProjectWithTasks(int projectId, int userId);
        Task<Project> CreateProject(CreateProjectDto projectDto, int userId);
        Task<bool> DeleteProject(int projectId, int userId);
    }

    public class ProjectService : IProjectService
    {
        private readonly AppDbContext _context;

        public ProjectService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ProjectDto>> GetProjectsForUser(int userId)
        {
            return await _context.Projects
                .Where(p => p.UserId == userId)
                .OrderByDescending(p => p.CreatedAt)
                .Select(p => new ProjectDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    CreatedAt = p.CreatedAt,
                    TaskCount = p.Tasks.Count,
                    CompletedTaskCount = p.Tasks.Count(t => t.IsCompleted)
                })
                .ToListAsync();
        }

        public async Task<ProjectDetailDto> GetProjectWithTasks(int projectId, int userId)
        {
            var project = await _context.Projects
                .Include(p => p.Tasks)
                .FirstOrDefaultAsync(p => p.Id == projectId && p.UserId == userId);

            if (project == null)
            {
                throw new KeyNotFoundException("Project not found");
            }

            return new ProjectDetailDto
            {
                Id = project.Id,
                Name = project.Name,
                Description = project.Description,
                CreatedAt = project.CreatedAt,
                TaskCount = project.Tasks.Count,
                CompletedTaskCount = project.Tasks.Count(t => t.IsCompleted),
                Tasks = project.Tasks.Select(t => new TaskDto
                {
                    Id = t.Id,
                    Title = t.Title,
                    DueDate = t.DueDate,
                    IsCompleted = t.IsCompleted,
                    ProjectId = t.ProjectId
                }).OrderBy(t => t.IsCompleted)
                  .ThenBy(t => t.DueDate ?? DateTime.MaxValue)
                  .ToList()
            };
        }

        public async Task<Project> CreateProject(CreateProjectDto projectDto, int userId)
        {
            var project = new Project
            {
                Name = projectDto.Name,
                Description = projectDto.Description,
                UserId = userId,
                CreatedAt = DateTime.UtcNow
            };

            _context.Projects.Add(project);
            await _context.SaveChangesAsync();
            return project;
        }

        public async Task<bool> DeleteProject(int projectId, int userId)
        {
            var project = await _context.Projects
                .FirstOrDefaultAsync(p => p.Id == projectId && p.UserId == userId);

            if (project == null)
            {
                return false;
            }

            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
