using Microsoft.AspNetCore.Mvc;
using WorkPlanner.API.Models;
using WorkPlanner.API.Services;

namespace WorkPlanner.API.Controllers;

[ApiController]
[Route("api/v1/projects/{projectId}/[controller]")]
public class SchedulerController : ControllerBase
{
    private readonly ILogger<SchedulerController> _logger;
    private readonly SchedulerService _schedulerService;

    public SchedulerController(ILogger<SchedulerController> logger, SchedulerService schedulerService)
    {
        _logger = logger;
        _schedulerService = schedulerService;
    }

    [HttpPost]
    public IActionResult Schedule(string projectId, [FromBody] ScheduleRequest request)
    {
        _logger.LogInformation("Received schedule request for project {ProjectId}", projectId);
        
        if (request == null)
        {
            return BadRequest("Request body cannot be empty");
        }

        var response = _schedulerService.ScheduleTasks(request);
        
        if (!string.IsNullOrEmpty(response.Error))
        {
            return BadRequest(new { error = response.Error });
        }

        return Ok(response);
    }
}
