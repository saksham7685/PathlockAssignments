using Microsoft.AspNetCore.Mvc;
using TaskManagerAPI.Models;
using TaskManagerAPI.Repositories;

namespace TaskManagerAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TasksController : ControllerBase
{
    private readonly ITaskRepository _taskRepository;
    private readonly ILogger<TasksController> _logger;

    public TasksController(ITaskRepository taskRepository, ILogger<TasksController> logger)
    {
        _taskRepository = taskRepository;
        _logger = logger;
    }

    // GET: api/tasks
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TaskItem>>> GetTasks()
    {
        var tasks = await _taskRepository.GetAllTasksAsync();
        return Ok(tasks);
    }

    // POST: api/tasks
    [HttpPost]
    public async Task<ActionResult<TaskItem>> CreateTask([FromBody] TaskItem taskItem)
    {
        if (string.IsNullOrWhiteSpace(taskItem.Description))
        {
            return BadRequest("Task description is required");
        }

        var createdTask = await _taskRepository.AddTaskAsync(taskItem);
        return CreatedAtAction(nameof(GetTask), new { id = createdTask.Id }, createdTask);
    }

    // GET: api/tasks/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<TaskItem>> GetTask(Guid id)
    {
        var task = await _taskRepository.GetTaskByIdAsync(id);
        if (task == null)
        {
            return NotFound();
        }
        return Ok(task);
    }

    // PUT: api/tasks/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTask(Guid id, [FromBody] TaskItem taskItem)
    {
        if (id != taskItem.Id)
        {
            return BadRequest("Task ID mismatch");
        }

        var existingTask = await _taskRepository.GetTaskByIdAsync(id);
        if (existingTask == null)
        {
            return NotFound();
        }

        var updatedTask = await _taskRepository.UpdateTaskAsync(taskItem);
        if (updatedTask == null)
        {
            return NotFound();
        }

        return NoContent();
    }

    // DELETE: api/tasks/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTask(Guid id)
    {
        var task = await _taskRepository.GetTaskByIdAsync(id);
        if (task == null)
        {
            return NotFound();
        }

        await _taskRepository.DeleteTaskAsync(id);
        return NoContent();
    }
}
