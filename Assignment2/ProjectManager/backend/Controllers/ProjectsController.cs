using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectManager.API.DTOs;
using ProjectManager.API.Services;

namespace ProjectManager.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/projects")]
    public class ProjectsController : ControllerBase
    {
        private readonly IProjectService _projectService;
        private readonly IAuthService _authService;

        public ProjectsController(IProjectService projectService, IAuthService authService)
        {
            _projectService = projectService;
            _authService = authService;
        }

        [HttpGet]
        public async Task<IActionResult> GetProjects()
        {
            var userId = _authService.GetCurrentUserId();
            var projects = await _projectService.GetProjectsForUser(userId);
            return Ok(projects);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProject(int id)
        {
            try
            {
                var userId = _authService.GetCurrentUserId();
                var project = await _projectService.GetProjectWithTasks(id, userId);
                return Ok(project);
            }
            catch (KeyNotFoundException)
            {
                return NotFound("Project not found");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateProject(CreateProjectDto projectDto)
        {
            var userId = _authService.GetCurrentUserId();
            var project = await _projectService.CreateProject(projectDto, userId);
            return CreatedAtAction(nameof(GetProject), new { id = project.Id }, project);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProject(int id)
        {
            var userId = _authService.GetCurrentUserId();
            var result = await _projectService.DeleteProject(id, userId);
            
            if (!result)
            {
                return NotFound("Project not found");
            }

            return NoContent();
        }
    }
}
