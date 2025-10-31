using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectManager.API.DTOs;
using ProjectManager.API.Services;

namespace ProjectManager.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/projects/{projectId}/tasks")]
    public class TasksController : ControllerBase
    {
        private readonly ITaskService _taskService;
        private readonly IAuthService _authService;

        public TasksController(ITaskService taskService, IAuthService authService)
        {
            _taskService = taskService;
            _authService = authService;
        }

        [HttpPost]
        public async Task<IActionResult> AddTask(int projectId, CreateTaskDto taskDto)
        {
            try
            {
                var userId = _authService.GetCurrentUserId();
                var task = await _taskService.AddTaskToProject(projectId, taskDto, userId);
                return CreatedAtAction(nameof(UpdateTask), new { projectId, taskId = task.Id }, task);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPut("{taskId}")]
        public async Task<IActionResult> UpdateTask(int projectId, int taskId, UpdateTaskDto taskDto)
        {
            var userId = _authService.GetCurrentUserId();
            var result = await _taskService.UpdateTask(taskId, taskDto, userId);
            
            if (!result)
            {
                return NotFound("Task not found");
            }

            return NoContent();
        }

        [HttpDelete("{taskId}")]
        public async Task<IActionResult> DeleteTask(int projectId, int taskId)
        {
            var userId = _authService.GetCurrentUserId();
            var result = await _taskService.DeleteTask(taskId, userId);
            
            if (!result)
            {
                return NotFound("Task not found");
            }

            return NoContent();
        }

        [HttpPost("{taskId}/toggle")]
        public async Task<IActionResult> ToggleTaskCompletion(int projectId, int taskId)
        {
            var userId = _authService.GetCurrentUserId();
            var result = await _taskService.ToggleTaskCompletion(taskId, userId);
            
            if (!result)
            {
                return NotFound("Task not found");
            }

            return NoContent();
        }
    }
}
